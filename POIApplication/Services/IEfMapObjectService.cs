using POIApplication.DTO;
using POIApplication.Entities;
using Object = POIApplication.Entities.Object;

namespace POIApplication.Services
{
    public interface IEfMapObjectService
    {
        Task<List<MapObject>> GetAllObject();
        Task<MapObject> GetOneObjectById(int id);
        Task<MapObject> AddObject(MapObjectDtoForCreate objectDto);
        Task UpdateObject(int id, MapObjectDtoForUpdate objectDto);
        Task DeleteObject(int id);
        Task AddRange(List<MapObject> mapObjects);
    }
}
