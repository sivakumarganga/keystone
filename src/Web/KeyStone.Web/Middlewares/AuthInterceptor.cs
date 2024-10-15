using KeyStone.Web.Common;
using KeyStone.Web.Common.Models;
using KeyStone.Web.StateFactory;
using System.Net.Http.Headers;

namespace KeyStone.Web.Middlewares
{
    public class AuthInterceptor : DelegatingHandler
    {
        private readonly ILogger<AuthInterceptor> _logger;
        private readonly ILocalStorageProvider _localStorage;
        public AuthInterceptor(ILogger<AuthInterceptor> logger, ILocalStorageProvider localStorage)
        {
            _logger = logger;
            _localStorage = localStorage;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                //TODO: add app loader logic
                var auth = await _localStorage.GetItemAsync<LoginResult>(Constants.TokenLocalStorageKey);
                if (auth is not null)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue(auth.TokenType, auth.AccessToken);
                }
                var response = await base.SendAsync(request, cancellationToken);
                return response;
            }
            finally
            {
                //TODO: handle/stop app loader, since the request was complete
            }
        }
    }
}
