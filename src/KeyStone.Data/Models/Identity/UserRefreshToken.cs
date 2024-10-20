using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KeyStone.Data.Models.Identity
{
    public class UserRefreshToken : IEntityTypeConfiguration<UserRefreshToken>
    {
        public UserRefreshToken()
        {
            CreatedAt = DateTime.Now;
        }
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public bool IsValid { get; set; }
        public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
        {
            builder.ToTable("UserRefreshTokens","usr");
        }
    }
}
