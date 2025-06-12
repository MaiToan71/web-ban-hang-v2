using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Configurations
{
    public class PostTypeImageConfiguration : IEntityTypeConfiguration<PostTypeImage>
    {
        public void Configure(EntityTypeBuilder<PostTypeImage> builder)
        {
            builder.HasKey(t => new { t.PostTypeId, t.ImageId });
            builder.ToTable("PostTypeImages");
            builder.HasOne(t => t.PostType).WithMany(pc => pc.PostTypeImages)
              .HasForeignKey(pc => pc.PostTypeId);
            builder.HasOne(t => t.Image).WithMany(pc => pc.PostTypeImages)
              .HasForeignKey(pc => pc.ImageId);
        }
    }
}
