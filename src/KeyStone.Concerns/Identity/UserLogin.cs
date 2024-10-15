using KeyStone.Concerns.Domain;
using Microsoft.AspNetCore.Identity;

namespace KeyStone.Concerns.Identity
{
    public class UserLogin : IdentityUserLogin<int>
    {
        public UserLogin()
        {
            LoggedOn = DateTime.Now;
        }

        public User User { get; set; } = null!;
        public DateTime LoggedOn { get; set; }
    }
}
