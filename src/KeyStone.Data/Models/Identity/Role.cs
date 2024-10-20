using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KeyStone.Data.Models.Identity
{
    public class Role : IdentityRole<int>, IEntityTypeConfiguration<Role>
    {
        public Role()
        {
            CreatedDate = DateTime.Now;
        }
        public int Id { get; set; }
        public string DisplayName { get; set; } = String.Empty;
        public DateTime CreatedDate { get; set; }
        public ICollection<RoleClaim> Claims { get; set; } = null!;
        public ICollection<UserRole> Users { get; set; } = null!;


        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles", "usr");
        }
    }
}
