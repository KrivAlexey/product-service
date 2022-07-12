using Microsoft.EntityFrameworkCore;

namespace ProductService.Dal;

public class ProductDbContext : DbContext
{
   public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
   {}
   
   public DbSet<Product>? Products { get; set; }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      modelBuilder.Entity<Product>(
         builder => builder.HasIndex(p => p.Name).IsUnique());
   }
}