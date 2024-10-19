using KeyStone.Concerns.Domain;
using KeyStone.Data.Models;

namespace KeyStone.Data.RepoContracts;

public interface IUserRefreshTokenRepository
{
    Task<Guid> CreateToken(int userId);
    Task<UserRefreshToken> GetTokenWithInvalidation(Guid id);
    Task<User> GetUserByRefreshToken(Guid tokenId);
    Task RemoveUserOldTokens(int userId, CancellationToken cancellationToken);
    void Update(UserRefreshToken entity);
}