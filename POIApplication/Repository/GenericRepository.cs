
using Microsoft.EntityFrameworkCore;
using POIApplication.Data;

namespace POIApplication.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected AppDbContext context;
        internal DbSet<T> dbSet;
        protected readonly ILogger _logger;

        public GenericRepository(AppDbContext context, ILogger logger)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
            this._logger = logger;
        }

        public async Task<bool> AddRange(IEnumerable<T> entities)
        {
            await dbSet.AddRangeAsync(entities);
            return true;
        }

        async Task<bool> IGenericRepository<T>.AddObject(T entity)
        {
            await dbSet.AddAsync(entity);
            return true;
        }

        async Task<bool> IGenericRepository<T>.DeleteObject(int id)
        {
            var entity = await dbSet.FindAsync(id);
            if (entity == null)
                return false;
            dbSet.Remove(entity);
            return true;
        }

        async Task<IEnumerable<T>> IGenericRepository<T>.GetAllObject()
        {
            return await dbSet.ToListAsync();
        }

        async Task<T> IGenericRepository<T>.GetObjectById(int id)
        {
            return await dbSet.FindAsync(id);
        }

        Task<bool> IGenericRepository<T>.UpdateObjectById(T entity)
        {
            dbSet.Update(entity);
            return Task.FromResult(true);
        }
    }
}
