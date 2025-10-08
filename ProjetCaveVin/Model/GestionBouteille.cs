using Microsoft.Data.SqlClient;
using ProjetCaveVin.Helpers;
using ProjetCaveVin.Model.Classes;
using ProjetCaveVin.Model.TonProjet.Models;
using ProjetCaveVin.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;



namespace ProjetCaveVin.Model
{
    internal class GestionBouteille : BaseViewModel
    {
        private ObservableCollection<Bouteille> _bouteilles;
        public ObservableCollection<Bouteille> Bouteilles
        {
            get => _bouteilles;
            set
            {
                _bouteilles = value;
                OnPropertyChanged(); // méthode héritée de BaseViewModel normalement
            }
        }

        public GestionBouteille()
        {
            // Charger la liste dès l'initialisation
            Bouteilles = new ObservableCollection<Bouteille>(GetBouteille());
        }

        private List<Bouteille> GetBouteille()
        {
            List<Bouteille> Bouteilles = new();
            string connectionString = @"Server=172.16.119.42\SQLEXPRESS02,1433;Database=cave;User Id=yohan;Password=1234;Encrypt=False;";

            var db = new DatabaseConnexion(connectionString);

            db.Open();

            using (var command = db.createCommand())
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
                                        t.LibelleType AS Type,
                                        z.Code AS ZoneCode,
                                        e.Code_Emplacement
                                    FROM Bouteille b
                                    INNER JOIN DernierDeplacement d 
                                        ON b.id_bouteille = d.id_bouteille AND d.rn = 1
                                    INNER JOIN Emplacement e 
                                        ON d.id_emplacement = e.id_emplacement
                                    INNER JOIN Zone z 
                                        ON e.id_zone = z.id_zone
                                    INNER JOIN TypeBouteille t
                                        ON b.id_type = t.id_type;
                                    ";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var bouteille = new Bouteille
                        {
                            Id = reader.GetInt32(0),
                            Libelle = reader.GetString(1),
                            Millesime = reader.GetString(2),
                            Type = reader.GetString(3),
                            Code = reader.GetString(4),
                            Code_Emplacement = reader.GetString(5),
                        };
                        Bouteilles.Add(bouteille);

                    }


                }
                return Bouteilles;


                
            }





        }
    }
}
