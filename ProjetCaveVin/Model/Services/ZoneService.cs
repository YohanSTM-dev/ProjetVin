using ProjetCaveVin.Model.Classes;
using ProjetCaveVin.Model.Connexion;
using System.Collections.Generic;

namespace ProjetCaveVin.Model.Services
{
    public class ZoneService : IZoneService
    {
        private readonly string _connectionString;

        public ZoneService()
        {
            _connectionString = @"Server=172.16.119.42\SQLEXPRESS02,1433;Database=cave;User Id=yohan;Password=1234;Encrypt=False;";
        }

        public List<Zone> GetAllZones()
        {
            List<Zone> zones = new();
            var db = new DatabaseConnexion(_connectionString);
            db.Open();

            using (var command = db.CreateCommand())
            {
                command.CommandText = @"
                    SELECT id_zone, Code
                    FROM Zone";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        zones.Add(new Zone
                        {
                            Id = reader.GetInt32(0),
                            Code = reader.GetString(1),
                        });
                    }
                }
            }

            return zones;
        }
    }
}