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
    public class AppUsermangersRoleConfiguration : IEntityTypeConfiguration<AppUsermangersRole>
    {
        public void Configure(EntityTypeBuilder<AppUsermangersRole> builder)
        {
            builder.HasKey(t => new { t.UserId, t.RoleId });
            builder.ToTable("AppUsermangersRoles");
            builder.HasOne(t => t.AppUser).WithMany(pc => pc.AppUsermangersRoles)
              .HasForeignKey(pc => pc.UserId);

            builder.HasOne(t => t.Role).WithMany(pc => pc.AppUsermangersRoles)
              .HasForeignKey(pc => pc.RoleId);
        }
    }
}
