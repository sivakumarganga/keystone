using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KeyStone.Data.Models.Identity
{
    public class UserToken : IdentityUserToken<int>, IEntityTypeConfiguration<UserToken>
    {
        public UserToken()
        {
            GeneratedTime = DateTime.Now;
        }
        public int Id { get; set; }
        public User User { get; set; } = null!;
        public DateTime GeneratedTime { get; set; }

        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.HasOne(u => u.User)
                   .WithMany(u => u.Tokens)
                   .HasForeignKey(u => u.UserId);

            builder.ToTable("UserTokens", "usr");
        }
    }
}
