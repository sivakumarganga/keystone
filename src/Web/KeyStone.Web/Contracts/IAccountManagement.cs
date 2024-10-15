using KeyStone.Web.Common.Models;

namespace KeyStone.Web.Contracts
{
    public interface IAccountManagement
    {
        Task<AuthenticationResult> LoginAsync(LoginRequest request);
        Task LogoutAsync();
        Task<bool> CheckAuthenticatedAsync();
    }
}
