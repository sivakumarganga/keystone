using Microsoft.AspNetCore.Identity;

namespace KeyStone.Concerns.Identity
{
    public class Role : IdentityRole<int>
    {
        public Role()
        {
            CreatedDate = DateTime.Now;
        }

        public string DisplayName { get; set; } = String.Empty;
        public DateTime CreatedDate { get; set; }
        public ICollection<RoleClaim> Claims { get; set; } = null!;
        public ICollection<UserRole> Users { get; set; } = null!;


    }
}
