using ProjetCaveVin.Model.Classes;
using ProjetCaveVin.Model.Connexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCaveVin.Repositories
{
    public class BouteilleRepository
    {
        public static List<Bouteille> getAllBouteilles()
        {
            var listBouteilles = new List<Bouteille>();
            var db = new DatabaseConnexion(DatabaseConnexion.ConnexionDatabase());
            db.Open();
            using (var command = db.CreateCommand())
            {
                command.CommandText = @"
                                SELECT 
                                    b.id_bouteille, 
                                    b.Libelle, 
                                    b.Millesime, 
                                    b.Contenance, 
                                    b.Prix,
                                    e.Code_Emplacement, 
                                    tb.libelleType, 
                                    b.id_origine 
                                FROM Bouteille b
                                INNER JOIN HistoriqueDeplacement h ON b.id_bouteille = h.id_bouteille
                                INNER JOIN Emplacement e ON h.id_emplacement = e.id_emplacement
                                INNER JOIN TypeBouteille tb on b.id_type = tb.id_type
                                WHERE h.Date_Deplacement = (
                                    SELECT MAX(h2.Date_Deplacement) 
                                    FROM HistoriqueDeplacement h2 
                                    WHERE h2.id_bouteille = b.id_bouteille
                                )";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listBouteilles.Add(new Bouteille
                        {
                            Id = reader.GetInt32(0),
                            Libelle = reader.GetString(1),
                            Millesime = reader.IsDBNull(2) ? "" : reader.GetString(2),
                            Contenance = reader.GetDecimal(3),
                            Prix = reader.GetDecimal(4),
                            Code_Emplacement = reader.GetString(5),

                            Type = reader.GetString(6),
                            //Photo = reader.IsDBNull(8) ? null : reader.GetString(8),
                            IdOrigine = reader.GetInt32(7)
                        });
                    }
                }
                return listBouteilles;
            }
        }
        public static void AjouterBouteille(Bouteille nouvelleBouteille, int idEmplacement, int idUtilisateur)
        {
            var db = new DatabaseConnexion(DatabaseConnexion.ConnexionDatabase());
            db.Open();

            using (var connection = db.CreateCommand().Connection)
            {
                var transaction = connection.BeginTransaction();

                try
                {
                    var cmdBouteille = connection.CreateCommand();
                    cmdBouteille.Transaction = transaction;
                    cmdBouteille.CommandText = @"
                INSERT INTO Bouteille (Libelle, Millesime, Contenance, Prix, Code, Type, Photo, id_origine)
                VALUES (@Lib, @Mil, @Cont, @Prix, @Code, @Type, @Photo, @Origine);
                SELECT SCOPE_IDENTITY();";

                    cmdBouteille.Parameters.AddWithValue("@Lib", nouvelleBouteille.Libelle);
                    cmdBouteille.Parameters.AddWithValue("@Mil", nouvelleBouteille.Millesime ?? (object)DBNull.Value);
                    cmdBouteille.Parameters.AddWithValue("@Cont", nouvelleBouteille.Contenance);
                    cmdBouteille.Parameters.AddWithValue("@Prix", nouvelleBouteille.Prix);
                    cmdBouteille.Parameters.AddWithValue("@Code", nouvelleBouteille.Code ?? "NO_CODE");
                    cmdBouteille.Parameters.AddWithValue("@Type", nouvelleBouteille.Type); // Attention, ici c'est peut-être id_type selon ta base
                    cmdBouteille.Parameters.AddWithValue("@Photo", nouvelleBouteille.Photo ?? (object)DBNull.Value);
                    cmdBouteille.Parameters.AddWithValue("@Origine", nouvelleBouteille.IdOrigine);

                    int newIdBouteille = Convert.ToInt32(cmdBouteille.ExecuteScalar());

                    //Insérer l'Historique - sinon aucun emplacement
                    var cmdHist = connection.CreateCommand();
                    cmdHist.Transaction = transaction;
                    cmdHist.CommandText = @"
                INSERT INTO HistoriqueDeplacement (Date_Deplacement, id_utilisateur, id_bouteille, id_emplacement)
                VALUES (GETDATE(), @User, @Bouteille, @Emplacement)";

                    cmdHist.Parameters.AddWithValue("@User", idUtilisateur);
                    cmdHist.Parameters.AddWithValue("@Bouteille", newIdBouteille);
                    cmdHist.Parameters.AddWithValue("@Emplacement", idEmplacement);

                    cmdHist.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    // si erreur , on annule tout meme la creation de la bouteille
                    transaction.Rollback();
                    throw;
                }
            }
            db.Close();
        }
        public static List<Bouteille> GetAllBouteillesAvecEmplacement()
        {
            var list = new List<Bouteille>();
            var db = new DatabaseConnexion(DatabaseConnexion.ConnexionDatabase());
            db.Open();

            using (var cmd = db.CreateCommand())
            {

                cmd.CommandText = @"
            SELECT 
                b.id_bouteille, 
                b.Libelle, 
                b.Millesime, 
                b.Contenance, 
                b.Prix, 
                b.id_origine,
                e.Code_Emplacement
            FROM Bouteille b
            INNER JOIN HistoriqueDeplacement h ON b.id_bouteille = h.id_bouteille
            INNER JOIN Emplacement e ON h.id_emplacement = e.id_emplacement

            -- FILTRE MAGIQUE : On ne garde que la ligne la plus récente pour chaque bouteille

            WHERE h.Date_Deplacement = (
                SELECT MAX(h2.Date_Deplacement) 
                FROM HistoriqueDeplacement h2 
                WHERE h2.id_bouteille = b.id_bouteille
            )";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Bouteille
                        {
                            Id = reader.GetInt32(0),
                            Libelle = reader.GetString(1),
                            Millesime = reader.IsDBNull(2) ? "" : reader.GetString(2),
                            Contenance = reader.GetDecimal(3),
                            Prix = reader.GetDecimal(4),
                            IdOrigine = reader.GetInt32(5),
                            Code_Emplacement = reader.GetString(6)
                        });
                    }
                }
            }
            db.Close();
            return list;
        }
    }
}
