using Microsoft.AspNetCore.Mvc;
using POIApplication.Response;
using POIApplication.Services;
using System.Diagnostics.Eventing.Reader;
using System.Xml.Linq;

namespace POIApplication.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DbObjectController : ControllerBase
    {
        private readonly IDbObjectService _dbObjectService;
        public DbObjectController(IDbObjectService dbObjectService)
        {
            _dbObjectService = dbObjectService;
        }
        [HttpPost]
        public Result AddObject(Entities.Object mapObject)
        {
            var result = new Result();
            if (String.IsNullOrEmpty(mapObject.Name))
            {
                result.Message = "İsim boş olamaz";
                return result;
            }
            if (mapObject.Name.Length > 100)
            {
                result.Message = "İsim maximum 100 karakter olmalı";
                return result;
            }
            if (mapObject == null)
            {

                result.Success = false;
                result.Message = "Poligon bulunamadı";
                result.Data = null;
                return result;
            }
            _dbObjectService.AddObject(mapObject);
            
                result.Success = true;
                result.Message = "Başarıyla eklendi";
                result.Data = mapObject;
                return result;
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
