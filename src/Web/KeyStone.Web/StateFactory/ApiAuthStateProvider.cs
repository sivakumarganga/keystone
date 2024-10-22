using KeyStone.Web.Common.Models;
using KeyStone.Web.Contracts;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using KeyStone.Web.Common;

namespace KeyStone.Web.StateFactory
{
    public class ApiAuthStateProvider : AuthenticationStateProvider, IAccountManagement
    {
        private readonly IAuthService _authService;
        private readonly ILocalStorageProvider _localStorage;
        private readonly ILogger<ApiAuthStateProvider> _logger;
        private bool _isAuthenticated;

        public ApiAuthStateProvider(IAuthService authService, ILocalStorageProvider localStorage, ILogger<ApiAuthStateProvider> logger)
        {
            _authService = authService;
            _localStorage = localStorage;
            _logger = logger;
        }
        
        public async Task<bool> CheckAuthenticatedAsync()
        {
            await GetAuthenticationStateAsync();
            return _isAuthenticated;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            _isAuthenticated = false;
            ClaimsPrincipal user = new(new ClaimsIdentity());
            if (await IsTokenExpiredAsync())
            {
                await ClearLocalContextAsync();
                return new AuthenticationState(user);
            }
            var userContext = await _localStorage.GetItemAsync<UserContext>(Constants.UserContextLocalStorageKey);
            if (userContext is not null)
            {
                _isAuthenticated = true;
                ClaimsIdentity identity = PrepareClaimsIdFromUserContext(userContext);
                user = new ClaimsPrincipal(identity);
                _isAuthenticated = true;
            }
            return new AuthenticationState(user);
        }

        public async Task<AuthenticationResult> LoginAsync(LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            if (response.IsSuccess && response.Data is not null)
            {
                response.Data.ExpiresOn = DateTime.Now.AddSeconds(response.Data.ExpiresIn);
                await _localStorage.SaveItemAsync(Constants.TokenLocalStorageKey, response.Data);
                await FetchAndSaveContextAsync();

                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                return AuthenticationResult.Success(response.Data);
            }
            return AuthenticationResult.Fail(response.Errors);
        }

        public async Task LogoutAsync()
        {
            try
            {
                await _authService.LogoutAsync();
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex.Message);
            }
            finally
            {
                await ClearLocalContextAsync();
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }
        }
        
        #region Private methods

        private static ClaimsIdentity PrepareClaimsIdFromUserContext(UserContext userContext)
        {
            return new ClaimsIdentity([ new Claim(ClaimTypes.Name, userContext.DisplayName),
                    new Claim(nameof(userContext.UserId),userContext.UserId.ToString()),
                    new Claim(nameof(userContext.UserKey), userContext.UserKey.ToString()),
                    new Claim(ClaimTypes.Role,userContext.Role)]
                , "apiauth");
        }

        private async Task<bool> IsTokenExpiredAsync()
        {
            var loginResult = await _localStorage.GetItemAsync<LoginResult>(Constants.TokenLocalStorageKey);
            return loginResult is null || !loginResult.ExpiresOn.HasValue || loginResult.ExpiresOn.Value < DateTime.Now;
        }

        private async Task FetchAndSaveContextAsync()
        {
            var response = await _authService.GetUserContextAsync();
            if (response.IsSuccess && response.Data is not null)
            {
                await _localStorage.SaveItemAsync(Constants.UserContextLocalStorageKey, response.Data);
            }
        }

        private async Task ClearLocalContextAsync()
        {
            await _localStorage.RemoveItemAsync(Constants.TokenLocalStorageKey);
            await _localStorage.RemoveItemAsync(Constants.UserContextLocalStorageKey);
        }

        #endregion
    }
}
