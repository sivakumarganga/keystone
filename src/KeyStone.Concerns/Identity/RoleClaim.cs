using Microsoft.AspNetCore.Identity;

namespace KeyStone.Concerns.Identity
{
    public class RoleClaim : IdentityRoleClaim<int>
    {
        public RoleClaim()
        {
            CreatedClaim = DateTime.Now;
        }

        public DateTime CreatedClaim { get; set; }
        public Role Role { get; set; } = null!;

    }
}
