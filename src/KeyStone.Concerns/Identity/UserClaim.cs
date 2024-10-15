using KeyStone.Concerns.Domain;
using Microsoft.AspNetCore.Identity;

namespace KeyStone.Concerns.Identity
{
    public class UserClaim : IdentityUserClaim<int>
    {
        public User User { get; set; } = null!;
    }
}
