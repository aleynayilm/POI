using Microsoft.AspNetCore.Mvc;
using POIApplication.Entities;
using System.Text.RegularExpressions;
using Object = POIApplication.Entities.Object;

namespace POIApplication.Services
{
    public class ObjectService : IObjectService
    {
        private static List<Object> _mapObject = new List<Object>();
        void IObjectService.Add(Object mapObject)
        {
            string pattern = @"^(\d+(\.\d+)?\s\d+(\.\d+)?)(,\s*\d+(\.\d+)?\s\d+(\.\d+)?)*$";
            bool validate = Regex.IsMatch(mapObject.WKT, pattern);
            if (validate)
            {
                _mapObject.Add(mapObject); 
            }
            else
            {
                throw new ArgumentException("Geçerli bir WKT giriniz");
            }
        }

        void IObjectService.AddRange([FromBody] List<Object> mapObjects)
        {
            string pattern = @"^(\d+(\.\d+)?\s\d+(\.\d+)?)(,\s*\d+(\.\d+)?\s\d+(\.\d+)?)*$";
            foreach (var obj in mapObjects)
            {
                if (!Regex.IsMatch(obj.WKT, pattern))
                    throw new ArgumentException("Geçerli bir WKT giriniz");
            }
            _mapObject.AddRange(mapObjects);
        }

        void IObjectService.DeleteById(int id)
        {
            var obj = _mapObject.FirstOrDefault(o => o.Id == id);
            if (obj != null)
                _mapObject.Remove(obj);
        }

        List<Object> IObjectService.GetAll() => _mapObject;

        Object IObjectService.GetById(int id) => _mapObject.FirstOrDefault(o=>o.Id==id);

        void IObjectService.Update(Object mapObject)
        {
            var existing= _mapObject.FirstOrDefault(o => o.Id == mapObject.Id);
            string pattern = @"^(\d+(\.\d+)?\s\d+(\.\d+)?)(,\s*\d+(\.\d+)?\s\d+(\.\d+)?)*$";
            if (existing != null)
            {
                existing.Name = mapObject.Name;
                existing.WKT = mapObject.WKT;
            }
            bool validate = Regex.IsMatch(mapObject.WKT, pattern);
            if (validate)
            {
                _mapObject.Add(mapObject);
            }
            else
            {
                throw new ArgumentException("Geçerli bir WKT giriniz");
            }
        }
    }
}