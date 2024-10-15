using KeyStone.Concerns.Domain;
using Microsoft.AspNetCore.Identity;

namespace KeyStone.Concerns.Identity
{
    public class UserRole : IdentityUserRole<int>
    {
        public User User { get; set; } = null!;
        public Role Role { get; set; } = null!;
        public DateTime CreatedUserRoleDate { get; set; }
    }
}
