using Microsoft.EntityFrameworkCore;
using FurnishMvc.Models;

namespace FurnishMvc.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Slider> Sliders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Product>()
            .Property(p => p.OldPrice)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Slider>()
            .Property(s => s.Price)
            .HasPrecision(18, 2);
    }
}
