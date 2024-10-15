using KeyStone.Web.Common.Models;
using KeyStone.Web.Contracts;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace KeyStone.Web.StateFactory
{
    public class ApiAuthStateProvider : AuthenticationStateProvider, IAccountManagement
    {
        public Task<bool> CheckAuthenticatedAsync()
        {
            //TODO: Implement authentication 
            return Task.FromResult(true);
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //TODO: fetch usercontext from local storage and prepare claims principal  
            ClaimsPrincipal user = new(new ClaimsIdentity());
            return Task.FromResult(new AuthenticationState(user));
        }

        public Task<AuthenticationResult> LoginAsync(LoginRequest request)
        {
            //TODO: Hit api and fetch tokens
            //save tokens to local storage
            //notify blazor that auth state has changed
            return Task.FromResult(AuthenticationResult.Success(new LoginResult()
            {
                AccessToken = "at",
                RefreshToken = "rt",
                ExpiresIn = 10000,
                ExpiresOn = DateTime.Now.AddSeconds(10000)
            }));
        }

        public Task LogoutAsync()
        {
            //TODO: Implement authentication 
            //TODO: Clear tokens and auth state from local storage 
            return Task.CompletedTask;
        }
    }
}
