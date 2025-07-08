using Microsoft.EntityFrameworkCore;
using NetTopologySuite.IO;
using POIApplication.Controllers;
using POIApplication.Entities;

namespace POIApplication.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
        public DbSet<MapObject> MapObjects { get;set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("postgis");
            modelBuilder.Entity<MapObject>().ToTable("mapobject");
            modelBuilder.Entity<MapObject>(entity =>
            {
                entity.Property(e => e.WKT)
                    .HasColumnType("geometry")
                    .HasColumnName("WKT")
                    .HasConversion(
                    wkt => string.IsNullOrEmpty(wkt) ? null : new WKTReader().Read(wkt),
                    geometry => geometry == null ? null : geometry.AsText()
                );
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
