using Microsoft.Data.SqlClient;
using ProjetCaveVin.Model.Classes;
using ProjetCaveVin.Model.Connexion;
using System.Collections.Generic;

namespace ProjetCaveVin.Model.Services
{
    public class BouteilleService : IBouteilleService
    {


        public List<Bouteille> GetAllBouteilles()
        {
            List<Bouteille> bouteilles = new();
            var db = new DatabaseConnexion(DatabaseConnexion.ConnexionDatabase());
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
                        ON b.id_type = t.id_type;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bouteilles.Add(new Bouteille
                        {
                            Id = reader.GetInt32(0),
                            Libelle = reader.GetString(1),
                            Millesime = reader.GetString(2),
                            Prix = reader.GetDecimal(3),
                            Type = reader.GetString(4),
                            Code = reader.GetString(5),
                            Code_Emplacement = reader.GetString(6),
                            Photo = reader.GetString(7),
                        });
                    }
                }
            }

            return bouteilles;
        }

        public List<Bouteille> GetBouteillesParZone(string codeZone)
        {
            List<Bouteille> bouteilles = new();
            var db = new DatabaseConnexion(DatabaseConnexion.ConnexionDatabase());
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
                    WHERE z.Code = @zone;";

                command.Parameters.AddWithValue("@zone", codeZone);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bouteilles.Add(new Bouteille
                        {
                            Id = reader.GetInt32(0),
                            Libelle = reader.GetString(1),
                            Millesime = reader.GetString(2),
                            Prix = reader.GetDecimal(3),
                            Type = reader.GetString(4),
                            Code = reader.GetString(5),
                            Code_Emplacement = reader.GetString(6),
                            Photo = reader.GetString(7),
                        });
                    }
                }
            }

            return bouteilles;
        }
    }
}