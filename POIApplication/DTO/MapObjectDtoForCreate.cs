using NetTopologySuite.Geometries;
using System.Text.Json.Serialization;

namespace POIApplication.DTO
{
    public class MapObjectDtoForCreate
    {
        [JsonPropertyName("wkt")]
        public string WKT { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
