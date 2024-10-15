using KeyStone.Concerns.Domain;
using Microsoft.AspNetCore.Identity;

namespace KeyStone.Concerns.Identity
{
    public class UserToken : IdentityUserToken<int>
    {
        public UserToken()
        {
            GeneratedTime = DateTime.Now;
        }

        public User User { get; set; } = null!;
        public DateTime GeneratedTime { get; set; }

    }
}
