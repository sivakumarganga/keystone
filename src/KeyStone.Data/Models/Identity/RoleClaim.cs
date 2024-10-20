using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KeyStone.Data.Models.Identity
{
    public class RoleClaim : IdentityRoleClaim<int>, IEntityTypeConfiguration<RoleClaim>
    {
        public RoleClaim()
        {
            CreatedClaim = DateTime.Now;
        }
        public DateTime CreatedClaim { get; set; }
        public Role Role { get; set; } = null!;

        public void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.ToTable("RoleClaims", "usr");

            builder.HasOne(u => u.Role)
                   .WithMany(u => u.Claims)
                   .HasForeignKey(u => u.RoleId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
