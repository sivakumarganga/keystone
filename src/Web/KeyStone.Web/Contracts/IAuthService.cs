using KeyStone.Web.Common.Models;

namespace KeyStone.Web.Contracts
{
    public interface IAuthService
    {
        Task<ApiResponse<LoginResult>> LoginAsync(LoginRequest loginModel);
        Task<ApiResponse<bool?>> LogoutAsync();
        Task<ApiResponse<UserContext>> GetUserContextAsync();
    }
}
