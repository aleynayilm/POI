using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using POIApplication.Entities;
using POIApplication.Response;
using POIApplication.Services;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Object = POIApplication.Entities.Object;

namespace POIApplication.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ObjectController:ControllerBase
    {
        private readonly IObjectService _service;
        public ObjectController()
        {
            _service = new ObjectService();
        }
        private static int _id = 1;

        [HttpPost]
        public Result AddObject(string wkt, string name)
        {
            var result = new Result();
            var mapObject = new Object
            {
                Id = _id++,
                WKT= wkt,
                Name= name
            };
            _service.Add(mapObject);
            result.Data = mapObject;
            result.Success = true;
            result.Message = "Poligon başarıyla eklendi";
            return result;
        }

        [HttpGet]
        public Result GetAll(){
            var result = new Result();
            var mapList= _service.GetAll();
            result.Data = mapList;
            result.Success = true;
            result.Message = "Poligonlar başarıyla listelendi";
            return result;
        }

        [HttpGet("{id}")]
        public Result GetObjectById([FromRoute(Name = "id")] int id)
        {
            var mapObject = _service.GetById(id);
            if (mapObject == null)
            {
                return new Result
                {
                    Success = false,
                    Message = "Poligon bulunamadı",
                    Data = null
                };
            }
            return new Result
            {
                Success = true,
                Message = "Poligon başarıyla bulundu",
                Data = mapObject
            };
        }

        [HttpDelete("{id}")]
        public Result DeleteObject([FromRoute(Name = "id")] int id)
        {
            var mapObject = _service.GetById(id);
            if (mapObject == null)
            {
                return new Result
                {
                    Success = false,
                    Message = "Poligon bulunamadı",
                    Data = null
                };
            }
            _service.DeleteById(id);
            return new Result
            {
                Success = true,
                Message = "Poligon başarıyla silindi",
                Data = mapObject
            };
        }

        [HttpPut("{id}")]
        public Result UpdateObject([FromRoute(Name = "id")] int id, string wkt, string name)
        {
            var mapObject = _service.GetById(id);
            if (mapObject == null)
            {
                return new Result
                {
                    Success = false,
                    Message = "Poligon bulunamadı",
                    Data = null
                };
            }
            mapObject.Name = name;
            mapObject.WKT = wkt;
            return new Result
            {
                Success = true,
                Message = "Başarıyla güncellendi",
                Data = mapObject
            };
        }

        [HttpPost]
        public Result AddRange([FromBody] List<Object> mapObjects)
        {
            _service.AddRange(mapObjects);
            if (mapObjects == null)
            {
                return new Result
                {
                    Success = false,
                    Message = "Liste bulunamadı",
                    Data = null
                };
            }
            return new Result
            {
                Success = true,
                Message = "liste başarıyla eklendi",
                Data = mapObjects
            };
        }
    }
}
