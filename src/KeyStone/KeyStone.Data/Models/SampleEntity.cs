using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyStone.Data.Models
{
    public class SampleEntity : IEntityTypeConfiguration<SampleEntity>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public void Configure(EntityTypeBuilder<SampleEntity> builder)
        {
            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Id)
                   .IsRequired();

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(500);

          

            // Table & Column Mappings
            builder.ToTable("SampleEntity");
            builder.Property(x => x.Id).HasColumnName("Id");
            builder.Property(x => x.Name).HasColumnName("Url");

            //  Relationships
            //  builder.HasOne(x => x.Post)
            //       .WithMany(x => x.Comments)
            //       .HasForeignKey(x => x.PostId)
            //       .IsRequired();
        }
    }
}
