using Microsoft.AspNetCore.Mvc;
using POIApplication.Response;
using POIApplication.Services;

namespace POIApplication.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DbController : ControllerBase
    {
        private readonly IDbObjectService _dbObjectService;

        public DbController(IDbObjectService dbObjectService)
        {
            _dbObjectService = dbObjectService;
        }

        [HttpPost]
        public Result AddObject(Entities.Object mapObject)
        {
            if (mapObject == null)
            {
                return new Result
                {
                    Success = false,
                    Message = "Poligon bulunamadı",
                    Data = null
                };
            }
            _dbObjectService.AddObject(mapObject);
            return new Result
            {
                Success = true,
                Message = "Başarıyla silindi",
                Data = mapObject
            };
        }

        [HttpGet]
        public Result GetAllObjects()
        {
            var result = new Result();
            var list= _dbObjectService.GetAllObject();
            result.Success = true;
            result.Message = "Başarıyla listelendi";
            result.Data = list;
            return result;
        }

        [HttpGet("{id}")]
        public Result GetObjectById([FromRoute(Name="id")] int id)
        {
            var mapObject = _dbObjectService.GetObjectById(id);
            if (mapObject == null)
            {
                return new Result
                {
                    Success = false,
                    Message = "Object bulunamadı",
                    Data = null
                };
            }
            return new Result
            {
                Success = true,
                Message = "Başarıyla bulundu",
                Data = mapObject
            };
        }

        [HttpDelete("{id}")]
        public Result DeleteObject([FromRoute(Name = "id")] int id)
        {
            var mapObject = _dbObjectService.GetObjectById(id);
            if (mapObject == null)
            {
                return new Result
                {
                    Success = false,
                    Message = "Poligon bulunamadı",
                    Data = null
                };
            }
            _dbObjectService.DeleteObjectById(id);
            return new Result
            {
                Success = true,
                Message = "Başarıyla silindi",
                Data = mapObject
            };
        }

        [HttpPut("{id}")]
        public Result UpdateObject([FromRoute(Name = "id")] int id, string wkt, string name)
        {
            var mapObject = _dbObjectService.GetObjectById(id);
            if (mapObject == null)
            {
                return new Result
                {
                    Success = false,
                    Message = "Poligon bulunamadı",
                    Data = null
                };
            }
            mapObject.WKT = wkt;
            mapObject.Name = name;
            _dbObjectService.UpdateObject(mapObject);
            return new Result
            {
                Success = true,
                Message = "Başarıyla güncellendi",
                Data = mapObject
            };
        }
    }
}
