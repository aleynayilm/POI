
using Npgsql;
using POIApplication.Response;
using System.Text.RegularExpressions;

namespace POIApplication.Services
{
    public class DbObjectService : IDbObjectService
    {
        private readonly string _connectionString;
        private static List<Entities.Object> _mapObject = new List<Entities.Object>();

        public DbObjectService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        void IDbObjectService.AddObject(Entities.Object mapObject)
        {
            ValidateWKT(mapObject.WKT, ()=>_mapObject.Add(mapObject));
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand("INSERT INTO mapobject (wkt, name) VALUES (@wkt, @name)", connection);
            cmd.Parameters.AddWithValue("wkt", mapObject.WKT);
            cmd.Parameters.AddWithValue("name", mapObject.Name);
            cmd.ExecuteNonQuery();
        }
        void IDbObjectService.UpdateObject(Entities.Object mapObject)
        {
            ValidateWKT(mapObject.WKT, () => _mapObject.Add(mapObject));
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand("UPDATE mapobject SET wkt=@wkt, name=@name WHERE id=@id", connection);
            cmd.Parameters.AddWithValue("id", mapObject.Id);
            cmd.Parameters.AddWithValue("wkt", mapObject.WKT);
            cmd.Parameters.AddWithValue("name", mapObject.Name);
            cmd.ExecuteNonQuery();
        }
        void IDbObjectService.AddRangeO(List<Entities.Object> mapObjects)
        {
        }

        void IDbObjectService.DeleteObjectById(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand("DELETE FROM mapobject WHERE id=@id", connection);
            cmd.Parameters.AddWithValue("id", id);
            cmd.ExecuteNonQuery();
        }

        List<Entities.Object> IDbObjectService.GetAllObject()
        {
            var list = new List<Entities.Object>();
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand("SELECT * FROM mapobject", connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Entities.Object
                {
                    Id = Convert.ToInt32(reader["id"]),
                    WKT = reader["wkt"].ToString(),
                    Name = reader["name"].ToString()
                });
            }

            return list;
        }

        Entities.Object IDbObjectService.GetObjectById(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand("SELECT * FROM mapobject WHERE id=@id", connection);
            cmd.Parameters.AddWithValue("id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Entities.Object
                {
                    Id = Convert.ToInt32(reader["id"]),
                    WKT = reader["wkt"].ToString(),
                    Name = reader["name"].ToString()
                };
            }

            return null;
        }
        private static void ValidateWKT(string wkt, Action executeIfValid)
        {
            string pattern = @"^(\d+(\.\d+)?\s\d+(\.\d+)?)(,\s*\d+(\.\d+)?\s\d+(\.\d+)?)*$";
            bool validate = Regex.IsMatch(wkt, pattern);

            if (validate)
            {
                executeIfValid();
            }
            else
            {
                throw new ArgumentException("Geçerli bir WKT giriniz");
            }
        }
    }
}