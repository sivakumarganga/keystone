using System.Security.Claims;
using System.Text;
using KeyStone.Concerns.Domain;
using KeyStone.Data.Models.Identity;
using KeyStone.Identity.Contracts;
using KeyStone.Identity.Dtos;
using KeyStone.Identity.Jwt;
using KeyStone.Identity.PermissionManagement;
using KeyStone.Identity.Seed;
using KeyStone.Identity.Wrappers;
using KeyStone.Identity.Wrappers.Extensions;
using KeyStone.Identity.Wrappers.Managers;
using KeyStone.Identity.Wrappers.Stores;
using KeyStone.Identity.Wrappers.Validators;
using KeyStone.Shared.API;
using KeyStone.Shared.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace KeyStone.Identity.ServiceConfiguration
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection RegisterIdentityServices(this IServiceCollection services, IdentitySettings identitySettings)
        {
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAppUserManager, AppUserManagerImplementation>();
            services.AddScoped<IIdentityDatabaseSeeder, IdentityDatabaseSeeder>();

            services.AddScoped<IUserValidator<User>, AppUserValidator>();
            services.AddScoped<UserValidator<User>, AppUserValidator>();

            services.AddScoped<IUserClaimsPrincipalFactory<User>, AppUserClaimsPrincipleFactory>();

            services.AddScoped<IRoleValidator<Role>, AppRoleValidator>();
            services.AddScoped<RoleValidator<Role>, AppRoleValidator>();

            services.AddScoped<IAuthorizationHandler, DynamicPermissionHandler>();
            services.AddScoped<IDynamicPermissionService, DynamicPermissionService>();
            services.AddScoped<IRoleStore<Role>, RoleStore>();
            services.AddScoped<IUserStore<User>, AppUserStore>();
            services.AddScoped<IRoleManagerService, RoleManagerService>();


            services.AddIdentity<User, Role>(options =>
            {
                options.Stores.ProtectPersonalData = false;

                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireUppercase = false;

                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = true;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = false;
                options.User.RequireUniqueEmail = false;

                //options.Stores.ProtectPersonalData = true;


            }).AddUserStore<AppUserStore>()
                .AddRoleStore<RoleStore>().
                //.AddUserValidator<AppUserValidator>().
                //AddRoleValidator<AppRoleValidator>().
                AddUserManager<AppUserManager>().
                AddRoleManager<AppRoleManager>().
                AddErrorDescriber<AppErrorDescriber>()
                //.AddClaimsPrincipalFactory<AppUserClaimsPrincipleFactory>()
                .AddDefaultTokenProviders().
                AddSignInManager<AppSignInManager>()
                .AddDefaultTokenProviders()
                .AddPasswordlessLoginTotpTokenProvider();

            services.AddAuthorizationCore(options =>
            {
                options.AddPolicy(ConstantPolicies.DynamicPermission, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.Requirements.Add(new DynamicPermissionRequirement());
                });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var secretKey = identitySettings.SecretKey;
                var encryptionKey = identitySettings.Encryptkey;

                var validationParameters = new TokenValidationParameters()
                {
                    ClockSkew = TimeSpan.Zero,
                    
                    //signature
                    RequireSignedTokens = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    
                    //lifetime
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    
                    //audience
                    ValidateAudience = true, //default : false
                    ValidAudience = identitySettings.Audience,
                    
                    //issuer
                    ValidateIssuer = true, //default : false
                    ValidIssuer = identitySettings.Issuer,

                    TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(encryptionKey)),
                };
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = validationParameters;
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context => Task.CompletedTask,
                    OnTokenValidated = async context =>
                    {
                        var signInManager = context.HttpContext.RequestServices.GetRequiredService<AppSignInManager>();

                        var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                        if (claimsIdentity.Claims?.Any() != true)
                            context.Fail("This token has no claims.");

                        var securityStamp =
                            claimsIdentity.FindFirstValue(new ClaimsIdentityOptions().SecurityStampClaimType);
                        if (!securityStamp.HasValue())
                            context.Fail("This token has no secuirty stamp");

                        //Find user and token from database and perform your custom validation
                        var userId = claimsIdentity.GetUserId<int>();
                        // var user = await userRepository.GetByIdAsync(context.HttpContext.RequestAborted, userId);

                        //if (user.SecurityStamp != Guid.Parse(securityStamp))
                        //    context.Fail("Token secuirty stamp is not valid.");

                        var validatedUser = await signInManager.ValidateSecurityStampAsync(context.Principal);
                        if (validatedUser == null)
                            context.Fail("Token secuirty stamp is not valid.");
                    },
                    OnChallenge = async context =>
                    {
                        if (context.AuthenticateFailure is SecurityTokenExpiredException)
                        {
                            context.HandleResponse();
                            
                            var response = new ApiResponse(false, "Token is expired. refresh your token");
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            await context.Response.WriteAsJsonAsync(response);
                        }

                        else if (context.AuthenticateFailure != null)
                        {
                            context.HandleResponse();

                            var response = new ApiResponse(false, "Token is Not Valid");
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            await context.Response.WriteAsJsonAsync(response);

                        }

                        else
                        {
                            context.HandleResponse();

                            context.Response.StatusCode = (int)StatusCodes.Status401Unauthorized;
                            await context.Response.WriteAsJsonAsync(new ApiResponse(false, "Invalid access token. Please login"));
                        }
                    },
                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = (int)StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsJsonAsync(new ApiResponse(false,"Forbidden"));
                    }
                };
                options.IncludeErrorDetails = true;
            });

            return services;
        }

        public static async Task SeedDefaultUsers(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();
            var seedService = scope.ServiceProvider.GetRequiredService<IIdentityDatabaseSeeder>();
            await seedService.Seed();
        }
    }
}
