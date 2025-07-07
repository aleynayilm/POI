using Microsoft.AspNetCore.Mvc;
using POIApplication.DTO;
using POIApplication.Entities;
using POIApplication.Response;
using POIApplication.Services;

namespace POIApplication.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MapObjectController : ControllerBase
    {
        private readonly IEfMapObjectService _efMapObjectService;

        public MapObjectController(IEfMapObjectService efMapObjectService)
        {
            _efMapObjectService = efMapObjectService;
        }
        [HttpGet]
        public async Task<Result> GetAll()
        {
            var result = new Result();
            var list = await _efMapObjectService.GetAllObject();
            result.Success = true;
            result.Message = "Başarıyla listelendi";
            result.Data = list;
            return result;
        }

        [HttpGet("{id}")]
        public async Task<Result> GetById(int id)
        {
            var obj = await _efMapObjectService.GetOneObjectById(id);
            var result = new Result();
            if (obj == null)
            {
                result.Success = false;
                result.Message = "Poligon bulunamadı";
                result.Data = null;
                return result;
            }
            result.Success = true;
            result.Message = "Başarıyla bulundu";
            result.Data = obj;
            return result;
        }

        [HttpPost]
        public async Task<Result> Add(MapObjectDtoForCreate dto)
        {
            var result = new Result();
            if (String.IsNullOrEmpty(dto.Name))
            {
                result.Message = "İsim boş olamaz";
                return result;
            }
            if (dto.Name.Length > 100)
            {
                result.Message = "İsim maximum 100 karakter olmalı";
                return result;
            }
            if (dto == null)
            {
                result.Success = false;
                result.Message = "Poligon bulunamadı";
                result.Data = null;
                return result;
            }
            var created = await _efMapObjectService.AddObject(dto);
            result.Success = true;
            result.Message = "Başarıyla eklendi";
            result.Data = created;
            return result;
        }

        [HttpPut("{id}")]
        public async Task<Result> Update([FromRoute(Name = "id")] int id, MapObjectDtoForUpdate dto)
        {
            var obj=await _efMapObjectService.GetOneObjectById(id);
            var result = new Result();
            if (dto == null)
            {
                result.Success = false;
                result.Message = "Poligon bulunamadı";
                result.Data = null;
            }
            await _efMapObjectService.UpdateObject(id, dto);

            var updatedObj = await _efMapObjectService.GetOneObjectById(id);
            result.Success = true;
            result.Message = "Başarıyla güncellendi";
            result.Data = updatedObj;
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<Result> Delete(int id)
        {
            var obj = await _efMapObjectService.GetOneObjectById(id);
            var result = new Result();
            if (obj == null)
            {
                result.Success = false;
                result.Message = "Poligon bulunamadı";
                result.Data = null;
            }
            await _efMapObjectService.DeleteObject(id);
            result.Success = true;
            result.Message = "Başarıyla silindi";
            result.Data = obj;
            return result;
        }

        [HttpPost("add-range")]
        public async Task<Result> AddRange(List<MapObject> objects)
        {
            var result = new Result();

            if (objects == null)
            {
                result.Success = false;
                result.Message = "Eklenecek veri bulunamadı.";
                result.Data = null;
                return result;
            }

            await _efMapObjectService.AddRange(objects);

            result.Success = true;
            result.Message = "Nesneler başarıyla eklendi.";
            result.Data = objects;
            return result;
        }
    }
}
