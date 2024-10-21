using System.Security.Claims;
using KeyStone.Data.Models.Identity;
using KeyStone.Shared.Models.Identity;

namespace KeyStone.Identity.Contracts
{
    public interface IJwtService
    {
        Task<AccessTokenResponse> GenerateAsync(User user);
        Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);
        Task<AccessTokenResponse> GenerateByPhoneNumberAsync(string phoneNumber);
        Task<AccessTokenResponse> RefreshToken(Guid refreshTokenId);
    }
}
