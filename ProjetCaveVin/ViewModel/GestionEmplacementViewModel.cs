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



    }
}
