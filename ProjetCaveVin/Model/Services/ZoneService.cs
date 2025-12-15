using ProjetCaveVin.Model.Classes;
using ProjetCaveVin.Model.Connexion;
using System.Collections.Generic;

namespace ProjetCaveVin.Model.Services
{
    public class ZoneService : IZoneService
    {


        public List<Zone> GetAllZones()
        {
            List<Zone> zones = new();
            var db = new DatabaseConnexion(DatabaseConnexion.ConnexionDatabase());
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