using POIApplication.Data;
using POIApplication.DTO;
using POIApplication.Entities;
using POIApplication.Response;
using POIApplication.UnitOfWork;
using System.Text.RegularExpressions;

namespace POIApplication.Services
{
    public class EfMapObjectService : IEfMapObjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EfMapObjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<MapObject> AddObject(MapObjectDtoForCreate objectDto)
        {
            var newObject = new MapObject
            {
                WKT = objectDto.WKT,
                Name = objectDto.Name
            };
            ValidateWKT(newObject.WKT);
            await _unitOfWork.MapObject.AddObject(newObject);
            await _unitOfWork.CompleteAsync();
            return newObject;
        }

        public async Task AddRange(List<MapObject> mapObjects)
        {
            foreach (var obj in mapObjects)
            {
                ValidateWKT(obj.WKT);
            }
            await _unitOfWork.MapObject.AddRange(mapObjects);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteObject(int id)
        {
            var obj = await _unitOfWork.MapObject.GetObjectById(id);
            if (obj == null) throw new Exception("Nesne bulunamadı");
            await _unitOfWork.MapObject.DeleteObject(obj.Id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<List<MapObject>> GetAllObject()
        {
            var result = await _unitOfWork.MapObject.GetAllObject();
            return result.ToList();
        }

        public async Task<MapObject> GetOneObjectById(int id)
        {
            var result = await _unitOfWork.MapObject.GetObjectById(id);
            if (result == null)
                throw new Exception("Nesne bulunamadı");
            return result;
        }

        public async Task UpdateObject(int id, MapObjectDtoForUpdate objectDto)
        {
            var result = await _unitOfWork.MapObject.GetObjectById(id);
            if (result == null)
                throw new Exception("Nesne bulunamadı");
            result.WKT = objectDto.WKT;
            result.Name = objectDto.Name;
            ValidateWKT(result.WKT);
            await _unitOfWork.MapObject.UpdateObjectById(result);
            await _unitOfWork.CompleteAsync();
        }

        private static void ValidateWKT(string wkt)
        {
            string pattern = @"^(\d+(\.\d+)?\s\d+(\.\d+)?)(,\s*\d+(\.\d+)?\s\d+(\.\d+)?)*$";
            bool validate = Regex.IsMatch(wkt, pattern);

            if (!validate)
            {
                throw new ArgumentException("Geçerli bir WKT giriniz");
            }
        }
    }
    }
