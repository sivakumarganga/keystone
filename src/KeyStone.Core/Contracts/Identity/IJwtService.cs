using KeyStone.Concerns.Domain;
using KeyStone.Shared.Models.Identity;
using System.Security.Claims;

namespace KeyStone.Core.Contracts.Identity
{
    public interface IJwtService
    {
        Task<AccessTokenResponse> GenerateAsync(User user);
        Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);
        Task<AccessTokenResponse> GenerateByPhoneNumberAsync(string phoneNumber);
        Task<AccessTokenResponse> RefreshToken(Guid refreshTokenId);
    }
}
