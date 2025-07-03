using Microsoft.AspNetCore.Mvc;
using POIApplication.Entities;
using POIApplication.Response;
using System.Security.Cryptography.X509Certificates;

namespace POIApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PointController:ControllerBase
    {
        private static readonly List<Point> pointList= new List<Point>();
        private static int _id = 1;

        [HttpPost()]
        public Result Add(string name, int x, int y) {
            var result = new Result();
            if (String.IsNullOrEmpty(name))
            {
                result.Message = "İsim boş olamaz";
                return result;
            }
            if (name.Length > 100)
            {
                result.Message = "İsim maximum 100 karakter olmalı";
                return result;
            }
            if (x < -180 || x > 180)
            {
                result.Message = "X koordinatı için verilen değer aralık dışı";
                return result;
            }
            if (y < -90 || y > 90)
            {
                result.Message = "Y koordinatı için verilen değer aralık dışı";
                return result;
            }
            var point = new Point
            {
                Id = _id++,
                Name = name,
                X = x,
                Y = y
            };
            pointList.Add(point);
            result.Data = point;
            result.Success = true;
            result.Message = "Nokta başarıyla eklendi";
            return result;
        }

        [HttpGet()]
        public List<Point> GetAll()
        {
            return pointList;
        }

        [HttpGet("{id}")]
        public Result GetPointById([FromRoute(Name = "id")] int id) { 
        var point= pointList.FirstOrDefault(p => p.Id == id);
            if (point == null)
            {
                return new Result
                {
                    Success = false,
                    Message = "Nokta bulunamadı",
                    Data = null
                };
            }
            return new Result
            {
                Success = true,
                Message = "Nokta başarıyla bulundu",
                Data = point
            };
        }

        [HttpDelete("{id}")]
        public Result DeleteOnePoint([FromRoute(Name = "id")] int id) {
            var point= pointList.FirstOrDefault(p => p.Id == id);
            if (point == null)
            {
                return new Result
                {
                    Success = false,
                    Message = "Nokta bulunamadı",
                    Data = null
                };
            }
            pointList.Remove(point);
            return new Result
            {
                Success = true,
                Message = "Nokta başarıyla silindi",
                Data = point
            };
        }

        [HttpPut("{id}")]
        public Result UpdateOnePoint([FromRoute(Name = "id")] int id, string name, int x, int y)
        {
            var point = pointList.FirstOrDefault(p => p.Id == id);
            if (point == null)
            {
                return new Result
                {
                    Success = false,
                    Message = "Nokta bulunamadı",
                    Data = null
                };
            }
            point.Name = name;
            point.X = x;
            point.Y = y;
            return new Result
            {
                Success = true,
                Message = "Nokta başarıyla güncellendi",
                Data = point
            };
        }
    }
}
