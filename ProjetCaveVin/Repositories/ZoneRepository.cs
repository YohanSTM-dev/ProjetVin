using ProjetCaveVin.Model.Connexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetCaveVin.Model.Classes;
using Microsoft.Data.SqlClient;
namespace ProjetCaveVin.Repositories
{
    public class ZoneRepository
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

        public void CreerEmplacement(int capacite, string nom, string zone)
        {
            DatabaseConnexion db = new(DatabaseConnexion.ConnexionDatabase());
            db.Open();
            using (var command = db.CreateCommand())
            {
                command.Parameters.AddWithValue("@nom", nom);
                command.Parameters.AddWithValue("@capacite", capacite);
                command.Parameters.AddWithValue("@zone", zone);
                command.CommandText = @"
                    INSERT INTO EMPLACEMENT VALUES (@nom,@capacite,0,@zone)
                    ";
                command.ExecuteNonQuery(); 


            }
            db.Close();
        }
    }
}
