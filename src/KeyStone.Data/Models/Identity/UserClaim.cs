using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KeyStone.Data.Models.Identity
{
    public class UserClaim : IdentityUserClaim<int>, IEntityTypeConfiguration<UserClaim>
    {
        public int Id { get; set; }
        public User User { get; set; } = null!;
        public void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.ToTable("UserClaims","usr");
        }
    }
}
