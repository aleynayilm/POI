using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
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
            var validateWkt= ValidateWKT(objectDto.WKT);
            var newObject = new MapObject
            {
                WKT = validateWkt,
                Name = objectDto.Name
            };
            await _unitOfWork.MapObject.AddObject(newObject);
            await _unitOfWork.CompleteAsync();
            return newObject;
        }

        public async Task AddRange(List<MapObject> mapObjects)
        {
            foreach (var obj in mapObjects)
            {
                obj.WKT= ValidateWKT(obj.WKT);
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
            result.WKT = ValidateWKT(objectDto.WKT);
            result.Name = objectDto.Name;
            await _unitOfWork.MapObject.UpdateObjectById(result);
            await _unitOfWork.CompleteAsync();
        }
private static string ValidateWKT(string wkt)
{
    if (string.IsNullOrWhiteSpace(wkt))
        throw new ArgumentException("WKT değeri boş olamaz");

    try
    {
        if (wkt.ToUpper().StartsWith("POLYGON"))
        {
            wkt = FixPolygonWkt(wkt);
        }
        
        var reader = new WKTReader();
        var geometry = reader.Read(wkt);
        
        return geometry.AsText();
    }
    catch (Exception ex)
    {
        throw new ArgumentException($"Geçerli bir WKT giriniz. Hata: {ex.Message}");
    }
}
        private static string FixPolygonWkt(string wkt)
{
    var polygonMatch = Regex.Match(wkt, @"POLYGON\s*\(\s*\(([^)]+)\)\s*\)", RegexOptions.IgnoreCase);
    if (!polygonMatch.Success)
        return wkt;
    
    var coordinates = polygonMatch.Groups[1].Value.Trim();
    var points = coordinates.Split(',');
    
    if (points.Length < 3)
        throw new ArgumentException("Polygon en az 3 nokta içermelidir");
    
    
    var firstPoint = points[0].Trim();
    var lastPoint = points[points.Length - 1].Trim();
    
    
    if (firstPoint != lastPoint)
    {
        coordinates += $", {firstPoint}";
    }
    
    return $"POLYGON (({coordinates}))";
}
    }
    }
