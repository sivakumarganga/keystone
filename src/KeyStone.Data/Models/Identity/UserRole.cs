using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KeyStone.Data.Models.Identity
{
    public class UserRole : IdentityUserRole<int>, IEntityTypeConfiguration<UserRole>
    {
        public virtual User User { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
        public DateTime CreatedUserRoleDate { get; set; }

        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasOne(userRole => userRole.Role)
            .WithMany(role => role.Users)
            .HasForeignKey(userRole => userRole.RoleId);

            builder.HasOne(userRole => userRole.User)
                .WithMany(user => user.UserRoles)
                .HasForeignKey(userRole => userRole.UserId);
            builder.ToTable("UserRoles", "usr");
        }
    }
}
