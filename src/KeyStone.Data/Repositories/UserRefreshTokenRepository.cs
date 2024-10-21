using EntityFrameworkCore.Repository;
using EntityFrameworkCore.Repository.Interfaces;
using KeyStone.Data.Models.Identity;
using KeyStone.Data.RepoContracts;
using Microsoft.EntityFrameworkCore;
using DM = KeyStone.Data.Models;

namespace KeyStone.Data.Repositories;

public class UserRefreshTokenRepository : Repository<UserRefreshToken>,IRepository<UserRefreshToken>,IAsyncRepository<UserRefreshToken>, IUserRefreshTokenRepository
{
    public UserRefreshTokenRepository(DbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Guid> CreateToken(int userId)
    {
        var token = new UserRefreshToken { IsValid = true, UserId = userId };
        await base.AddAsync(token);
        return token.Id;
    }

    public async Task<UserRefreshToken> GetTokenWithInvalidation(Guid id)
    {
        var token = await base.DbContext.Set<UserRefreshToken>().Where(t => t.IsValid && t.Id.Equals(id)).FirstOrDefaultAsync();

        return token;
    }

    public async Task<User> GetUserByRefreshToken(Guid tokenId)
    {
        var user = await base.DbContext.Set<UserRefreshToken>().AsNoTracking().Include(t => t.User).Where(c => c.Id.Equals(tokenId))
            .Select(c => c.User).FirstOrDefaultAsync();

        return user;
    }

    public Task RemoveUserOldTokens(int userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public void Update(UserRefreshToken entity)
    {
        base.DbContext.Set<UserRefreshToken>().Update(entity);
    }
}