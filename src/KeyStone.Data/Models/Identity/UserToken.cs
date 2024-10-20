using Microsoft.AspNetCore.Identity;

namespace KeyStone.Data.Models.Identity
{
    public class UserToken : IdentityUserToken<int>
    {
        public UserToken()
        {
            GeneratedTime = DateTime.Now;
        }
        public int Id { get; set; }
        public User User { get; set; } = null!;
        public DateTime GeneratedTime { get; set; }

    }
}
