using Microsoft.EntityFrameworkCore;
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
            modelBuilder.Entity<MapObject>().ToTable("mapobject");
        }
    }
}
