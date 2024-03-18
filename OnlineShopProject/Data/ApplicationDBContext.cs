using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineShopProject.Models;

namespace OnlineShopProject.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<OrderProduct> OrderProduct { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderProduct>()
            .HasKey(op => new { op.OrderId, op.ProductId });

            modelBuilder.Entity<Cart>()
            .HasKey(pc => new { pc.ProductId, pc.AppUserId });

            List<IdentityRole<int>> roles = new List<IdentityRole<int>>
            {
                new IdentityRole<int>
                {
                    Id = -1,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole<int>
                {
                    Id = -2,
                    Name = "User",
                    NormalizedName = "USER"
                },
            };
            modelBuilder.Entity<IdentityRole<int>>().HasData(roles);
        }
    }
}
