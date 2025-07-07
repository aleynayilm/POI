using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace POIApplication.Entities
{
    [Table("mapobject")]
    public class MapObject
    {
        public int Id { get; set; }
        public string WKT { get; set; }
        public string Name { get; set; }
    }
}
