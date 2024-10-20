using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KeyStone.Data.Models.Identity
{
    public class UserClaim : IdentityUserClaim<int>, IEntityTypeConfiguration<UserClaim>
    {
        public User User { get; set; } = null!;
        public void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.HasOne(u => u.User)
                   .WithMany(u => u.Claims)
                   .HasForeignKey(u => u.UserId);
            builder.ToTable("UserClaims", "usr");
        }
    }
}
