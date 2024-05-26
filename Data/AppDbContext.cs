using APITEST.Models;
using Microsoft.EntityFrameworkCore;

namespace APITEST.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Incoming> Incomings { get; set; }
        public DbSet<Outgoing> Outgoings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Incomings)
                .WithOne(e => e.Product)
                .HasForeignKey(e => e.ProductId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Outgoings)
                .WithOne(s => s.Product)
                .HasForeignKey(s => s.ProductId);
        }
        public DbSet<APITEST.Models.ProductCategory> ProductCategories { get; set; } = default!;
    }
}
