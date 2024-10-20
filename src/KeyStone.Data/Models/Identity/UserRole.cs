using Microsoft.AspNetCore.Identity;

namespace KeyStone.Data.Models.Identity
{
    public class UserRole : IdentityUserRole<int>
    {
        public int Id { get; set; }
        public User User { get; set; } = null!;
        public Role Role { get; set; } = null!;
        public DateTime CreatedUserRoleDate { get; set; }
    }
}
