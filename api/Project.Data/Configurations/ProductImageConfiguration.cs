using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Data.Entities;

namespace Project.Data.Configurations
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.HasKey(t => new { t.ImageId, t.ProductId });
            builder.ToTable("ProductImages");
            builder.HasOne(t => t.Products).WithMany(pc => pc.ProductImages)
              .HasForeignKey(pc => pc.ProductId);

            builder.HasOne(t => t.Images).WithMany(pc => pc.ProductImages)
              .HasForeignKey(pc => pc.ImageId);

        }
    }
}
