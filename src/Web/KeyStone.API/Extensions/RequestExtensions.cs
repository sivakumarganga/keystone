using KeyStone.Core.Context;
using System.Security.Claims;

namespace KeyStone.API.Extensions
{
    public static class RequestExtensions
    {
        public static IServiceCollection ConfigureRequestContext(this IServiceCollection services)
        {
            services.AddScoped<IRequestContext>(sp =>
            {
                var httpContextAccessor = services.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>();
                var context = httpContextAccessor.HttpContext;
                RequestContext requestContext = new RequestContext();
                if (context is not null && context.User?.Identity?.IsAuthenticated == true)
                {
                    var userIdentity = context.User.Identity as ClaimsIdentity;
                    if(userIdentity is null)
                    {
                        return requestContext;
                    }

                    var role = userIdentity.FindFirst(ClaimTypes.Role)?.Value;
                    requestContext = new RequestContext()
                    {
                        UserName = userIdentity.Name,
                        DisplayName = userIdentity.FindFirst("displayName")?.Value ?? userIdentity.Name,
                        UserId = userIdentity.GetUserId<int>(),
                        Role = role,
                    };
                }
                return requestContext;
            });
            return services;
        }
    }
}
