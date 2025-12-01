using ProjetCaveVin.Model.Classes;
using ProjetCaveVin.Model.Connexion;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ProjetCaveVin.Helpers;


namespace ProjetCaveVin.ViewModel
{
    public class GestionEmplacementViewModel : BaseViewModel
    {
        private ObservableCollection<Zone> _zones;
        public ObservableCollection<Zone> Zones
        {
            get => _zones;
            set
            {
                _zones = value;
                OnPropertyChanged(); // méthode héritée de BaseViewModel normalement
            }
        }

        public GestionEmplacementViewModel()
        {
            // Charger la liste dès l'initialisation
            Zones = new ObservableCollection<Zone>(GetZones());
        }

        private List<Zone> GetZones()
        {
            List<Zone> Zones = new();
            string connectionString = @"Server=172.16.119.42\SQLEXPRESS02,1433;Database=cave;User Id=yohan;Password=1234;Encrypt=False;";
            //string connectionString = @"Server=localhost\SQLEXPRESS;Database=cave;User Id=pol;Password=1234;Encrypt=False;";
            var db = new DatabaseConnexion(connectionString);

            db.Open();

            using (var command = db.CreateCommand())
            {
                command.CommandText = @"                                   
                                    SELECT id_zone, Code
                                    FROM Zone
                                    ";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var zone = new Zone
                        {
                            Id = reader.GetInt32(0),
                            Code = reader.GetString(1),
                        };
                        Zones.Add(zone);

                    }


                }
                return Zones;



            }

        }



        public List<Bouteille> GetBouteillesParZone(string codeZone)
        {
            List<Bouteille> bouteilles = new();
            string connectionString = @"Server=172.16.119.42\SQLEXPRESS02,1433;Database=cave;User Id=yohan;Password=1234;Encrypt=False;";
            //string connectionString = @"Server=localhost\SQLEXPRESS;Database=cave;User Id=pol;Password=1234;Encrypt=False;";

            var db = new DatabaseConnexion(connectionString);
            db.Open();

            using (var command = db.CreateCommand())
            {
                command.CommandText = @"
            WITH DernierDeplacement AS (
                SELECT 
                    h.id_bouteille,
                    h.id_emplacement,
                    h.Date_Deplacement,
                    ROW_NUMBER() OVER (PARTITION BY h.id_bouteille ORDER BY h.Date_Deplacement DESC) AS rn
                FROM HistoriqueDeplacement h
            )
            SELECT 
                b.id_bouteille,
                b.Libelle,
                b.Millesime,
                b.Prix,
                t.LibelleType AS Type,
                z.Code AS ZoneCode,
                e.Code_Emplacement,
                t.PhotoURL
            FROM Bouteille b
            INNER JOIN DernierDeplacement d 
                ON b.id_bouteille = d.id_bouteille AND d.rn = 1
            INNER JOIN Emplacement e 
                ON d.id_emplacement = e.id_emplacement
            INNER JOIN Zone z 
                ON e.id_zone = z.id_zone
            INNER JOIN TypeBouteille t
                ON b.id_type = t.id_type
            WHERE z.Code = @zone;
        ";

                command.Parameters.AddWithValue("@zone", codeZone);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var bouteille = new Bouteille
                        {
                            Id = reader.GetInt32(0),
                            Libelle = reader.GetString(1),
                            Millesime = reader.GetString(2),
                            Prix = reader.GetDecimal(3),
                            Type = reader.GetString(4),
                            Code = reader.GetString(5),
                            Code_Emplacement = reader.GetString(6),
                            Photo = reader.GetString(7),
                        };

                        bouteilles.Add(bouteille);
                    }
                }
            }

            return bouteilles;
        }



    }
}
