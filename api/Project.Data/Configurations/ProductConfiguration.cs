using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Data.Entities;

namespace Project.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {

        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(i => i.Id);

            builder.HasOne(x => x.User).WithMany(x => x.Products).HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.PostType).WithMany(x => x.Products).HasForeignKey(x => x.PostTypeId);
        }
    }
}
