using Microsoft.EntityFrameworkCore;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ProductService.Dal;

public class ProductDbContext : DbContext
{
   public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
   {}
   
   #nullable disable
   public DbSet<Product> Products { get; set; }
   #nullable restore
   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      modelBuilder.Entity<Product>(
         builder => builder.HasIndex(p => p.Name).IsUnique());
   }
}