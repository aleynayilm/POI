using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System.ComponentModel.DataAnnotations.Schema;

namespace POIApplication.Entities
{
    [Table("mapobject")]
    public class MapObject
    {
        public int Id { get; set; }
        public string WKT { get; set; }
        [NotMapped]
        internal Geometry InternalGeometry
        {
            get => string.IsNullOrEmpty(WKT) ? null : new WKTReader().Read(WKT);
            set => WKT = value?.AsText();
        }
        public string Name { get; set; }
    }
}
