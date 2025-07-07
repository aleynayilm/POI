using POIApplication.Data;
using POIApplication.Repository;

namespace POIApplication.UnitOfWork
{
    public class UnitOfWork: IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;
        public IMapObjectRepository MapObject { get; private set; }
        public UnitOfWork(AppDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("logs");
            MapObject = new MapObjectRepository(context, _logger);
        }
        public async Task CompleteAsync() {
            await _context.SaveChangesAsync();
        }
        public void Dispose() {
            _context.Dispose();
        }
    }
}
