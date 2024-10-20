using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KeyStone.Data.Models.Identity
{
    public class UserLogin : IdentityUserLogin<int>, IEntityTypeConfiguration<UserLogin>
    {
        public UserLogin()
        {
            LoggedOn = DateTime.Now;
        }
        public int Id { get; set; }

        public User User { get; set; } = null!;
        public DateTime LoggedOn { get; set; }
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.HasOne(u => u.User)
                .WithMany(u => u.Logins)
                .HasForeignKey(u => u.UserId);
            builder.ToTable("UserLogins", "usr");
        }
    }
}
