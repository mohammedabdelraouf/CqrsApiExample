namespace CqrsApiExample.Data
{
    using CqrsApiExample.Models;
    using Microsoft.EntityFrameworkCore;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seeding initial data
            modelBuilder.Entity<Product>().HasData(
                new Product(1, "Laptop", "High performance laptop", 1200.50m),
                new Product(2, "Smartphone", "Latest Android smartphone", 800.00m),
                new Product(3, "Headphones", "Noise-cancelling headphones", 199.99m),
                new Product(4, "Monitor", "27-inch 4K monitor", 350.75m)
            );
        }

    }
}