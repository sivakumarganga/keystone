using FluentResults;
using KeyStone.Shared.API.RequestModels;
using KeyStone.Shared.Models;
using KeyStone.Shared.Models.Identity;

namespace KeyStone.Identity.Contracts
{
    public interface IAccountContract
    {
        Task<Result<bool>> SignUpAsync(SignUpRequest request);
        Task<Result<AccessTokenResponse>> GetTokenAsync(LoginRequest request);
        Task<Result<UserInfo>> GetUserInfoAsync();
        Task<Result<bool>> RequestLogoutAsync();
        Task<Result<AccessTokenResponse>> RefreshUserTokenAsync(Guid refreshToken);  
    }
}
