using Asp.Versioning;
using KeyStone.Shared.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Pluralize.NET;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Reflection;
using System.Security.Claims;

namespace KeyStone.API.Extensions
{
    public static class WebInfraExtensions
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));

            //More info : https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters


            //Add services and configuration to use swagger
            services.AddSwaggerGen(options =>
            {
                var xmlDocPath = Path.Combine(AppContext.BaseDirectory, "Project.xml");
                //show controller XML comments like summary
                options.IncludeXmlComments(xmlDocPath, true);

                options.EnableAnnotations();

                #region DescribeAllEnumsAsStrings
                //This method was Deprecated. 

                //You can specify an enum to convert to/from string, uisng :
                //[JsonConverter(typeof(StringEnumConverter))]
                //public virtual MyEnums MyEnum { get; set; }

                //Or can apply the StringEnumConverter to all enums globaly, using :
                //SerializerSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                //OR
                //JsonConvert.DefaultSettings = () =>
                //{
                //    var settings = new JsonSerializerSettings();
                //    settings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                //    return settings;
                //};
                #endregion

                //options.DescribeAllParametersInCamelCase();
                //options.DescribeStringEnumsInCamelCase()
                //options.UseReferencedDefinitionsForEnums()
                //options.IgnoreObsoleteActions();
                //options.IgnoreObsoleteProperties();

                options.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "API V1" });
                options.SwaggerDoc("v1.1", new OpenApiInfo { Version = "v1.1", Title = "API V1.1 using minimal API endpoints" });

                #region Filters
                //Enable to use [SwaggerRequestExample] & [SwaggerResponseExample]
                //options.ExampleFilters();

                //It doesn't work anymore in recent versions because of replacing Swashbuckle.AspNetCore.Examples with Swashbuckle.AspNetCore.Filters
                //Adds an Upload button to endpoints which have [AddSwaggerFileUploadButton]
                //options.OperationFilter<AddFileParamTypesOperationFilter>();

                //Set summary of action if not already set
                options.OperationFilter<ApplySummariesOperationFilter>();

                #region Add UnAuthorized to Response
                //Add 401 response and security requirements (Lock icon) to actions that need authorization
                #endregion

                #region Add Jwt Authentication
                //Add Lockout icon on top of swagger ui page to authenticate
                #region Old way
                //options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                //{
                //    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                //    Name = "Authorization",
                //    In = "header"
                //});
                //options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                //{
                //    {"Bearer", new string[] { }}
                //});
                #endregion

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Exclude Bearer in token, Example: \" {token}\"",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();
               //TODO options.OperationFilter<CustomTokenRequiredOperationFilter>();


                //OAuth2Scheme
                //options.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme());
                #endregion

                #region Versioning
                // Remove version parameter from all Operations
                options.OperationFilter<RemoveVersionParameters>();

                //set version "api/v{version}/[controller]" from current swagger doc verion
                options.DocumentFilter<SetVersionInPaths>();

                //Seperate and categorize end-points by doc version
                options.DocInclusionPredicate((docName, apiDesc) =>
                {

                    var endpointApiVersions = apiDesc.ActionDescriptor.EndpointMetadata.OfType<ApiVersionMetadata>();

                    foreach (var endpointApiVersion in endpointApiVersions)
                    {
                        endpointApiVersion.Deconstruct(out _, out var endpointModel);

                        return endpointModel.DeclaredApiVersions.Any(version => $"v{version}" == docName);

                    }

                    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

                    var versions = methodInfo.DeclaringType
                        .GetCustomAttributes<ApiVersionAttribute>(true)
                        .SelectMany(attr => attr.Versions);

                    return versions.Any(v => $"v{v}" == docName);
                });
                #endregion

                #endregion
            });
        }

        public static void UseSwaggerAndUI(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app, nameof(app));

            //More info : https://github.com/domaindrivendev/Swashbuckle.AspNetCore

            //Swagger middleware for generate "Open API Documentation" in swagger.json
            app.UseSwagger(options =>
            {
                //   options.RouteTemplate = "api-docs/{documentName}/swagger.json";
            });

            //Swagger middleware for generate UI from swagger.json
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
                options.SwaggerEndpoint("/swagger/v1.1/swagger.json", "V1.1 Docs Using Minimal Api Endpoints");

                #region Customizing
                //// Display
                //options.DefaultModelExpandDepth(2);
                //options.DefaultModelRendering(ModelRendering.Model);
                //options.DefaultModelsExpandDepth(-1);
                //options.DisplayOperationId();
                //options.DisplayRequestDuration();
                options.DocExpansion(DocExpansion.None);
                //options.EnableDeepLinking();
                //options.EnableFilter();
                //options.MaxDisplayedTags(5);
                //options.ShowExtensions();

                //// Network
                //options.EnableValidator();
                //options.SupportedSubmitMethods(SubmitMethod.Get);

                //// Other
                //options.DocumentTitle = "CustomUIConfig";
                //options.InjectStylesheet("/ext/custom-stylesheet.css");
                //options.InjectJavascript("/ext/custom-javascript.js");
                //options.RoutePrefix = "api-docs";
                #endregion
            });

            //ReDoc UI middleware. ReDoc UI is an alternative to swagger-ui
            app.UseReDoc(options =>
            {
                options.SpecUrl("/swagger/v1/swagger.json");
                //options.SpecUrl("/swagger/v2/swagger.json");

                #region Customizing
                //By default, the ReDoc UI will be exposed at "/api-docs"
                //options.RoutePrefix = "docs";
                //options.DocumentTitle = "My API Docs";

                options.EnableUntrustedSpec();
                options.ScrollYOffset(10);
                options.HideHostname();
                options.HideDownloadButton();
                options.ExpandResponses("200,201");
                options.RequiredPropsFirst();
                options.NoAutoAuth();
                options.PathInMiddlePanel();
                options.HideLoading();
                options.NativeScrollbars();
                options.DisableSearch();
                options.OnlyRequiredInSamples();
                options.SortPropsAlphabetically();
                #endregion
            });
        }

        public static IServiceCollection AddWebFrameworkServices(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                //url segment => {version}
                options.AssumeDefaultVersionWhenUnspecified = true; //default => false;
                options.DefaultApiVersion = new ApiVersion(1, 0); //v1.0 == v1
                options.ReportApiVersions = true;

                //ApiVersion.TryParse("1.0", out var version10);
                //ApiVersion.TryParse("1", out var version1);
                //var a = version10 == version1;

                //options.ApiVersionReader = new QueryStringApiVersionReader("api-version");
                // api/posts?api-version=1

                //options.ApiVersionReader = new UrlSegmentApiVersionReader();
                // api/v1/posts

                //options.ApiVersionReader = new HeaderApiVersionReader(new[] { "Api-Version" });
                // header => Api-Version : 1

                //options.ApiVersionReader = new MediaTypeApiVersionReader()

                //options.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader("api-version"), new UrlSegmentApiVersionReader())
                // combine of [querystring] & [urlsegment]
            });
            //services.AddSingleton<IAppContext>(sp => {
            //    var context = new ApplicationContext();
            //    context.AppTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
            //    return context;
            //});
            //services.AddTransient<IRequestContext>(sp =>
            //{

            //    var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
            //    var context = httpContextAccessor.HttpContext;
            //    if (context != null && context.User.Identity.IsAuthenticated)
            //    {
            //        var userIdentity = context.User.Identity as ClaimsIdentity;
            //        var role = userIdentity.FindFirst(ClaimTypes.Role)?.Value;
            //        var agencyClaim = userIdentity.FindFirst("agencyId");
            //        var userContext = new RequestContext()
            //        {
            //            UserName = userIdentity.Name,
            //            DisplayName = userIdentity.FindFirst("displayName")?.Value ?? userIdentity.Name,
            //            UserId = userIdentity.GetUserId<int>(),
            //            Role = role,
            //        };
            //        if (agencyClaim != null)
            //        {
            //            userContext.AgentId = int.Parse(userIdentity.FindFirst("agentId")?.Value ?? "0");
            //            userContext.AgencyId = int.Parse(agencyClaim.Value);
            //            userContext.AgencyName = userIdentity.FindFirst("agencyName")?.Value ?? string.Empty;
            //            userContext.AgencyCode = userIdentity.FindFirst("agencyCode")?.Value ?? string.Empty;
            //        }
            //        return userContext;
            //    }
            //    return new RequestContext();

            //});
            return services;


        }

    }

    public class RemoveVersionParameters : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Remove version parameter from all Operations
            var versionParameter = operation.Parameters.SingleOrDefault(p => p.Name == "version");
            if (versionParameter != null)
                operation.Parameters.Remove(versionParameter);
        }
    }
    public class SetVersionInPaths : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var updatedPaths = new OpenApiPaths();

            foreach (var entry in swaggerDoc.Paths)
            {
                updatedPaths.Add(
                    entry.Key.Replace("v{version}", swaggerDoc.Info.Version),
                    entry.Value);
            }

            swaggerDoc.Paths = updatedPaths;
        }
    }
    public class ApplySummariesOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var controllerActionDescriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor == null) return;

            var pluralizer = new Pluralizer();

            var actionName = controllerActionDescriptor.ActionName;
            var singularizeName = pluralizer.Singularize(controllerActionDescriptor.ControllerName);
            var pluralizeName = pluralizer.Pluralize(singularizeName);

            var parameterCount = operation.Parameters.Where(p => p.Name != "version" && p.Name != "api-version").Count();

            if (IsGetAllAction())
            {
                if (!operation.Summary.HasValue())
                    operation.Summary = $"Returns all {pluralizeName}";
            }
            else if (IsActionName("Post", "Create"))
            {
                if (!operation.Summary.HasValue())
                    operation.Summary = $"Creates a {singularizeName}";

                if (!operation.Parameters[0].Description.HasValue())
                    operation.Parameters[0].Description = $"A {singularizeName} representation";
            }
            else if (IsActionName("Read", "Get"))
            {
                if (!operation.Summary.HasValue())
                    operation.Summary = $"Retrieves a {singularizeName} by unique id";

                if (!operation.Parameters[0].Description.HasValue())
                    operation.Parameters[0].Description = $"a unique id for the {singularizeName}";
            }
            else if (IsActionName("Put", "Edit", "Update"))
            {
                if (!operation.Summary.HasValue())
                    operation.Summary = $"Updates a {singularizeName} by unique id";

                //if (!operation.Parameters[0].Description.HasValue())
                //    operation.Parameters[0].Description = $"A unique id for the {singularizeName}";

                if (!operation.Parameters[0].Description.HasValue())
                    operation.Parameters[0].Description = $"A {singularizeName} representation";
            }
            else if (IsActionName("Delete", "Remove"))
            {
                if (!operation.Summary.HasValue())
                    operation.Summary = $"Deletes a {singularizeName} by unique id";

                if (!operation.Parameters[0].Description.HasValue())
                    operation.Parameters[0].Description = $"A unique id for the {singularizeName}";
            }

            #region Local Functions
            bool IsGetAllAction()
            {
                foreach (var name in new[] { "Get", "Read", "Select" })
                {
                    if ((actionName.Equals(name, StringComparison.OrdinalIgnoreCase) && parameterCount == 0) ||
                        actionName.Equals($"{name}All", StringComparison.OrdinalIgnoreCase) ||
                        actionName.Equals($"{name}{pluralizeName}", StringComparison.OrdinalIgnoreCase) ||
                        actionName.Equals($"{name}All{singularizeName}", StringComparison.OrdinalIgnoreCase) ||
                        actionName.Equals($"{name}All{pluralizeName}", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                return false;
            }

            bool IsActionName(params string[] names)
            {
                foreach (var name in names)
                {
                    if (actionName.Equals(name, StringComparison.OrdinalIgnoreCase) ||
                        actionName.Equals($"{name}ById", StringComparison.OrdinalIgnoreCase) ||
                        actionName.Equals($"{name}{singularizeName}", StringComparison.OrdinalIgnoreCase) ||
                        actionName.Equals($"{name}{singularizeName}ById", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                return false;
            }
            #endregion
        }
    }
}
