using KeyStone.Concerns.Domain;
using KeyStone.Data;
using KeyStone.Data.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace KeyStone.Identity.Wrappers.Stores
{
    public class AppUserStore : UserStore<User, Role, KeyStoneDbContext, int, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>
    {
        public AppUserStore(KeyStoneDbContext context, IdentityErrorDescriber describer = null)
            : base(context, describer)
        {
        }
    }
}
