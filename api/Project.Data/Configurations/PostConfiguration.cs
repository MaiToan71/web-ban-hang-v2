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
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {

        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Posts");
            builder.HasKey(i => i.Id);

            builder.HasOne(x => x.User).WithMany(x => x.Posts).HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.PostType).WithMany(x => x.Posts).HasForeignKey(x => x.PostTypeId);
        }
    }
}
