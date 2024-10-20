using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KeyStone.Data.Models.Identity
{
    public class User : IdentityUser<int>, IEntityTypeConfiguration<User>
    {
        public User()
        {
            this.UserCode = Guid.NewGuid().ToString().Substring(0, 8);
        }
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string FamilyName { get; set; } = String.Empty;
        public string UserCode { get; set; } = String.Empty;

        public ICollection<UserRole> UserRoles { get; set; } = null!;
        public ICollection<UserLogin> Logins { get; set; } = null!;
        public ICollection<UserClaim> Claims { get; set; } = null!;
        public ICollection<UserToken> Tokens { get; set; } = null!;
        public ICollection<UserRefreshToken> UserRefreshTokens { get; set; } = null!;
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users","usr");
        }
    }
}
