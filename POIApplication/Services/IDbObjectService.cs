using POIApplication.Entities;
using Object = POIApplication.Entities.Object;

namespace POIApplication.Services
{
    public interface IDbObjectService
    {
        List<Object> GetAllObject();
        Object GetObjectById(int id);
        void AddObject(Object mapObject);
        void UpdateObject(Object mapObject);
        void DeleteObjectById(int id);
        void AddRangeO(List<Object> mapObjects);
    }
}
