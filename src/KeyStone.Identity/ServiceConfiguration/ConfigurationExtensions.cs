using KeyStone.Concerns.Domain;
using KeyStone.Concerns.Identity;
using KeyStone.Core.Contracts.Identity;
using KeyStone.Identity.Dtos;
using KeyStone.Identity.Jwt;
using KeyStone.Identity.PermissionManagement;
using KeyStone.Identity.Seed;
using KeyStone.Identity.Wrappers;
using KeyStone.Identity.Wrappers.Extensions;
using KeyStone.Identity.Wrappers.Managers;
using KeyStone.Identity.Wrappers.Stores;
using KeyStone.Identity.Wrappers.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

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

            return services;
        }
    }
}
