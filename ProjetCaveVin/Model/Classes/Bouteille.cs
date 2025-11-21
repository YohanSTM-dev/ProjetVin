using ProjetCaveVin.Model.Connexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCaveVin.Model.Classes
{
    public class Bouteille
    {
        public int Id { get; set; }
        public string Libelle { get; set; }
        public string Millesime { get; set; }
        public decimal Contenance { get; set; }
        public decimal Prix { get; set; }
        //------------------------------------------
        public string Code { get; set; }
        public string Code_Emplacement { get; set; }
        public string Type { get; set; }
        public string Photo { get; set; }
        //------------------------------------------
        public int IdOrigine { get; set; }
        public Origine Origine { get; set; }

        public List<HistoriqueDeplacement> HistoriqueDeplacements { get; set; } = new List<HistoriqueDeplacement>();
        private static string ConnectionString =>
        @"Server=localhost\SQLEXPRESS;Database=Cave;Trusted_Connection=True;Encrypt=False;";

        private static string ConnectionStringLocal => @"Server=172.16.119.42\SQLEXPRESS02,1433;Database=Cave;User Id=yohan;Password=1234;Encrypt=False;";



        public static List<Bouteille> getAllBouteilles()
        {   
            var listBouteilles = new List<Bouteille>();
            var db = new DatabaseConnexion(ConnectionString);
            db.Open();
            using (var command = db.CreateCommand())
            {
                command.CommandText = "SELECT id_bouteille, Libelle, Millesime, Contenance, Prix, Code, Code_Emplacement, Type, Photo, id_origine FROM Bouteille";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listBouteilles.Add(new Bouteille
                        {
                            Id = reader.GetInt32(0),
                            Libelle = reader.GetString(1),
                            Millesime = reader.GetString(2),
                            Contenance = reader.GetDecimal(3),
                            Prix = reader.GetDecimal(4),
                            Code = reader.GetString(5),
                            Code_Emplacement = reader.GetString(6),
                            Type = reader.GetString(7),
                            Photo = reader.GetString(8),
                            IdOrigine = reader.GetInt32(9)
                        });
                    }
                }
            }
            return listBouteilles;
        }
    }




}
