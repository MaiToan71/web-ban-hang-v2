using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Data.Configurations;
using Project.Data.Entities;

namespace Project.Data.EF
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure using Fluent API
            modelBuilder.ApplyConfiguration(new AppRoleConfiguration());
            modelBuilder.ApplyConfiguration(new AppUserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfuguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new AppUsermangersRoleConfiguration());
            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());
            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new PostTypeImageConfiguration());
            modelBuilder.ApplyConfiguration(new ImageConfiguration());
            modelBuilder.ApplyConfiguration(new PostImageConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationUserConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new RoleMenuConfiguration());
            modelBuilder.ApplyConfiguration(new MenuConfiguration());


            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductImageConfiguration());
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => x.UserId);
        }
        public DbSet<BankUser> BankUsers { get; set; }
        public DbSet<BankConfig> BankConfigs { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<RoleMenu> RoleMenus { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationUser> NotificationUsers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<AppUsermangersRole> AppUsermangersRoles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<UserDepartment> UserDepartments { get; set; }
        public DbSet<PostType> PostTypes { get; set; }
        public DbSet<PostTypeImage> PostTypeImages { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<PostImage> PostImages { get; set; }
        public DbSet<TimeShift> TimeShifts { get; set; }
        public DbSet<TimeShiftType> TimeShiftTypes { get; set; }
        public DbSet<TimeShiftUser> TimeShiftUsers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrdersDetail { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<WorkRecord> WorkRecords { get; set; }
        public DbSet<UserHistory> UserHistories { get; set; }
        public DbSet<PostDepartment> PostDepartments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductCategory> ProductCategoríes { get; set; }
        public DbSet<ProductCategoryReference> ProductCategoryReferences { get; set; }
        public DbSet<ProductImportReference> ProductImportReferences { get; set; }
        public DbSet<ProductHistory> ProductHistories { get; set; }
        public DbSet<ProductImport> ProductImports { get; set; }

        public DbSet<Coefficient> Coefficients { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<UserCoefficient> UserCoefficients { get; set; }

        public DbSet<UserService> UserServices { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<Project.Data.Entities.Attribute> Attributes { get; set; }
        public DbSet<Project.Data.Entities.Config> Configs { get; set; }

    }
}
