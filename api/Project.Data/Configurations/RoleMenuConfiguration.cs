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
    public class RoleMenuConfiguration : IEntityTypeConfiguration<RoleMenu>
    {
        public void Configure(EntityTypeBuilder<RoleMenu> builder)
        {
            builder.HasKey(t => new { t.RoleId, t.MenuId });
            builder.ToTable("RoleMenus");
            builder.HasOne(t => t.Role).WithMany(pc => pc.RoleMenus)
            .HasForeignKey(pc => pc.RoleId);

            builder.HasOne(t => t.Menu).WithMany(pc => pc.RoleMenus)
           .HasForeignKey(pc => pc.MenuId);

        }
    }
}
