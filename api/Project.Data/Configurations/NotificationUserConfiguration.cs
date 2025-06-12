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
    public class NotificationUserConfiguration : IEntityTypeConfiguration<NotificationUser>
    {
        public void Configure(EntityTypeBuilder<NotificationUser> builder)
        {
            builder.HasKey(t => new { t.NotificationId, t.UserId });
            builder.ToTable("NotificationUsers");
            builder.HasOne(t => t.Notification).WithMany(pc => pc.NotificationUsers)
              .HasForeignKey(pc => pc.NotificationId);

            builder.HasOne(t => t.AppUser).WithMany(pc => pc.NotificationUsers)
              .HasForeignKey(pc => pc.UserId);
        }
    }
}
