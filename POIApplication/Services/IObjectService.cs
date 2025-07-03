using POIApplication.Entities;
using Object = POIApplication.Entities.Object;

namespace POIApplication.Services
{
    public interface IObjectService
    {
        List<Object> GetAll();
        Object GetById(int id);
        void Add(Object mapObject);
        void Update(Object mapObject);
        void DeleteById(int id);
        void AddRange(List<Object> mapObjects);
    }
}
