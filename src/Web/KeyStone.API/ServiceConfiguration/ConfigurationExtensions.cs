using KeyStone.API.Middlewares;

namespace KeyStone.API.ServiceConfiguration
{
    public static class ConfigurationExtensions
    {
        public static WebApplication ConfigureCustomMiddlewares(this WebApplication app)
        {
            app.UseMiddleware<RequestIdMiddleware>();
            return app;
        }
    }
}
