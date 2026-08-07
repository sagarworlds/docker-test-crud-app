using CrudApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudApp.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            // PostgreSQL uses 'numeric' for decimals — HasPrecision works identically
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.Category).HasMaxLength(50);
            // PostgreSQL timestamps are stored as UTC by default with Npgsql
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        });
    }
}
