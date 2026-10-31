using Microsoft.EntityFrameworkCore;
using Snapnest.Core.Entities;

namespace Snapnest.Infrastructure.Data;

public class SnapnestDbContext : DbContext
{
    public DbSet<ImageItem> ImageItems { get; set; }
    public DbSet<Clothesline> Clotheslines { get; set; }
    public DbSet<ClotheslineImage> ClotheslineImages { get; set; }

    public SnapnestDbContext(DbContextOptions<SnapnestDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ClotheslineImage>().HasKey(ci => new { ci.ClotheslineId, ci.ImageId });
        modelBuilder.Entity<ClotheslineImage>().HasOne(ci => ci.Clothesline).WithMany(c => c.ClotheslineImages).HasForeignKey(ci => ci.ClotheslineId);
        modelBuilder.Entity<ClotheslineImage>().HasOne(ci => ci.ImageItem).WithMany().HasForeignKey(ci => ci.ImageId);
    }
}