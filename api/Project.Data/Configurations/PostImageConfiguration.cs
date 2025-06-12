using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Project.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Configurations
{
    public class PostImageConfiguration : IEntityTypeConfiguration<PostImage>
    {
        public void Configure(EntityTypeBuilder<PostImage> builder)
        {
            builder.HasKey(t => new { t.ImageId, t.PostId });
            builder.ToTable("PostImages");
            builder.HasOne(t => t.Posts).WithMany(pc => pc.PostImages)
              .HasForeignKey(pc => pc.PostId);

            builder.HasOne(t => t.Images).WithMany(pc => pc.PostImages)
              .HasForeignKey(pc => pc.ImageId);

        }
    }
}
