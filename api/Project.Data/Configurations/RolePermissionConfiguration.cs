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
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.HasKey(t => new { t.RoleId, t.PermissionId });
            builder.ToTable("RolePermissions");
            builder.HasOne(t => t.Role).WithMany(pc => pc.RolePermissions)
              .HasForeignKey(pc => pc.RoleId);

            builder.HasOne(t => t.Permission).WithMany(pc => pc.RolePermissions)
              .HasForeignKey(pc => pc.PermissionId);
        }
    }
}
