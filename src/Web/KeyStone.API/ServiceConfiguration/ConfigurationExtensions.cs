using FluentValidation;
using KeyStone.API.Middlewares;
using KeyStone.API.RequestValidators;
using KeyStone.Shared.API.RequestModels;

namespace KeyStone.API.ServiceConfiguration
{
    public static class ConfigurationExtensions
    {
        public static WebApplication ConfigureCustomMiddlewares(this WebApplication app)
        {
            app.UseMiddleware<RequestIdMiddleware>();
            return app;
        }

        public static IServiceCollection ConfigureRequestValidators(this IServiceCollection services)
        {
            services.AddTransient<IValidator<SignUpRequest>, SignUpRequestValidator>();
            services.AddTransient<IValidator<LoginRequest>, LoginRequestValidator>();

            return services;
        }
    }
}
