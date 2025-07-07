using Microsoft.EntityFrameworkCore;
using POIApplication.Data;
using POIApplication.Entities;

namespace POIApplication.Repository
{
    public class MapObjectRepository : GenericRepository<MapObject>, IMapObjectRepository
    {
        public MapObjectRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
