using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BnsBazarApp.Models;

namespace BnsBazarApp.Models.Data
{
    public class BnsBazarDbContext : DbContext
    {
        public BnsBazarDbContext(DbContextOptions<BnsBazarDbContext> options)
            : base(options)
        {
        }

        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔐 Admin seed using BCRYPT (same as login)
            var admin = new AppUser
            {
                Id = 1,
                Email = "bjgautam21@gmail.com",
                Role = "Admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("bijaya21"),
                CreatedAt = DateTime.UtcNow
            };

            modelBuilder.Entity<AppUser>().HasData(admin);

            // Price precision
            modelBuilder.Entity<Advertisement>()
                .Property(a => a.Price)
                .HasPrecision(18, 2);
        }
    }
}