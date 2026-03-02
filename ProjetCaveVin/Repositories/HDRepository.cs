using ProjetCaveVin.Model.Classes;
using ProjetCaveVin.Model.Connexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCaveVin.Repositories
{
    public class HDRepository
    {
        public static void EnregistrerMouvement(int idBouteille, int idNouveauEmplacement, int idUtilisateur)
        {
            var db = new DatabaseConnexion(DatabaseConnexion.ConnexionDatabase());
            db.Open();

            try
            {
                using (var command = db.CreateCommand())
                {
                    command.CommandText = @"
                        INSERT INTO HistoriqueDeplacement (Date_Deplacement,id_utilisateur,id_bouteille, id_emplacement )
                        VALUES (@Date, @IdUser,@IdBouteille, @IdEmplacement )";

                    command.Parameters.AddWithValue("@Date", DateTime.Now);
                    command.Parameters.AddWithValue("@IdUser", idUtilisateur);
                    command.Parameters.AddWithValue("@IdBouteille", idBouteille);
                    command.Parameters.AddWithValue("@IdEmplacement", idNouveauEmplacement);

                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                db.Close();
            }
        }


        public static List<HistoriqueDeplacement> GetAllHistoriqueDeplacements()
        {
            var listeRetour = new List<HistoriqueDeplacement>();

            var db = new DatabaseConnexion(DatabaseConnexion.ConnexionDatabase());
            db.Open();

            using (var command = db.CreateCommand())
            {
                command.CommandText = "SELECT id_historique, Date_Deplacement, id_utilisateur, id_bouteille, id_emplacement FROM HistoriqueDeplacement ORDER BY Date_Deplacement DESC";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var historique = new HistoriqueDeplacement
                        {
                            Id = reader.GetInt32(0),
                            DateDeplacement = reader.GetDateTime(1),
                            IdUtilisateur = reader.GetInt32(2),
                            IdBouteille = reader.GetInt32(3),
                            IdEmplacement = reader.GetInt32(4)
                        };

                        listeRetour.Add(historique);
                    }
                }
            }
            db.Close();

            return listeRetour;
        }
    }
}
