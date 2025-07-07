using System.Linq.Expressions;

namespace POIApplication.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllObject();
        Task<T> GetObjectById(int id);
        Task<bool> AddObject(T entity);
        Task<bool> DeleteObject(int id);
        Task<bool> UpdateObjectById(T entity);
        Task<bool> AddRange(IEnumerable<T> entities);
    }
}
