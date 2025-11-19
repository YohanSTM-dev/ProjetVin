using ProjetCaveVin.Model.Connexion;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace ProjetCaveVin.Model.Tables
{
    public class Utilisateur
    {
        public int id_utilisateur { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }   
        public Role Role { get; set; }

        private static string ConnectionString =>
            @"Server=localhost\SQLEXPRESS;Database=Cave;Trusted_Connection=True;Encrypt=False;";

        private static string ConnectionStringLocal => @"Server=172.16.119.42\SQLEXPRESS02,1433;Database=Cave;User Id=yohan;Password=1234;Encrypt=False;";

        public Utilisateur() { }

        public Utilisateur(int id, string nom, string prenom, string email, string passwordHash, string salt, Role role)
        {
            id_utilisateur = id;
            Nom = nom;
            Prenom = prenom;
            Email = email;
            PasswordHash = passwordHash;
            Salt = salt;
            Role = role;
        }

        //  Récupère tous les utilisateurs
        public static List<Utilisateur> GetAllUtilisateur()
        {
            var utilisateurs = new List<Utilisateur>();

            var db = new DatabaseConnexion(ConnectionString);
            db.Open();

            using (var command = db.CreateCommand())
            {
                command.CommandText = @"
                    SELECT u.id_utilisateur, u.Nom, u.Prenom, u.Email, u.PasswordHash, u.Salt, 
                           r.id_role, r.nom
                    FROM Utilisateur u
                    INNER JOIN Role r ON u.id_role_utilisateur = r.id_role";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var utilisateur = new Utilisateur
                        {
                            id_utilisateur = reader.GetInt32(0),
                            Nom = reader.GetString(1),
                            Prenom = reader.GetString(2),
                            Email = reader.GetString(3),
                            PasswordHash = reader.GetString(4),
                            Salt = reader.IsDBNull(5) ? null : reader.GetString(5),
                            Role = new Role
                            {
                                id_role = reader.GetInt32(6),
                                Nom = reader.GetString(7)
                            }
                        };
                        utilisateurs.Add(utilisateur);
                    }
                }
            }

            db.Close();
            return utilisateurs;
        }

        //  Vérifie les identifiants
        public static Utilisateur GetByCredentials(string email, string password)
        {
            var db = new DatabaseConnexion(ConnectionString);
            db.Open();

            try
            {
                using (var command = db.CreateCommand())
                {
                    // 1) Récupérer l'utilisateur par email
                    command.CommandText = @"
                    SELECT id_utilisateur, u.Nom, Prenom, Email, PasswordHash, Salt, r.id_role, r.nom
                    FROM Utilisateur u
                    INNER JOIN Role r ON u.id_role_utilisateur = r.id_role
                    WHERE u.Email = @Email";
                    command.Parameters.AddWithValue("@Email", email);

                    using var reader = command.ExecuteReader();
                    if (!reader.Read())
                        return null; // utilisateur inexistant

                    string storedHash = reader.GetString(reader.GetOrdinal("PasswordHash"));
                    string storedSalt = reader.IsDBNull(reader.GetOrdinal("Salt")) ? null : reader.GetString(reader.GetOrdinal("Salt"));


                    // 2) Vérifier le mot de passe avec le helper
                    bool isValid = false;
                    if (!string.IsNullOrEmpty(storedSalt))
                    {
                        isValid = PasswordHelper.VerifyPassword(password, storedHash, storedSalt);
                    }
                    else
                    {
                        // cas legacy si tu veux comparer directement le hash ou texte en clair
                        isValid = storedHash == password;
                    }

                    if (!isValid)
                        return null;

                    // 3) Retourner l'utilisateur si mot de passe correct
                    return new Utilisateur
                    {
                        id_utilisateur = reader.GetInt32(reader.GetOrdinal("id_utilisateur")),
                        Nom = reader.GetString(reader.GetOrdinal("Nom")),
                        Prenom = reader.GetString(reader.GetOrdinal("Prenom")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        PasswordHash = storedHash,
                        Role = new Role
                        {
                            id_role = reader.GetInt32(reader.GetOrdinal("id_role")),
                            Nom = reader.GetString(reader.GetOrdinal("nom"))
                        }
                    };
                }
            }
            finally
            {
                db.Close();
            }
        }



        // 🔹 Insère un nouvel utilisateur (avec hash + salt)
    public static void InsertUtilisateur(string nom, string prenom, string email, string plainPassword, string nomRole)
    {
        var (hash, salt) = PasswordHelper.HashPassword(plainPassword); // génère hash + salt
        int roleId;

        var db = new DatabaseConnexion(ConnectionString);
        db.Open();

        // Récupérer l'id du rôle
        using (var cmd = db.CreateCommand())
        {
            cmd.CommandText = "SELECT id_role FROM Role WHERE nom = @Role";
            cmd.Parameters.AddWithValue("@Role", nomRole);
            var result = cmd.ExecuteScalar();
            if (result == null)
                throw new Exception("Rôle invalide.");
            roleId = Convert.ToInt32(result);
        }

        // Insérer l'utilisateur avec hash + salt
        using (var cmdInsert = db.CreateCommand())
        {
            cmdInsert.CommandText = @"
            INSERT INTO Utilisateur (Nom, Prenom, Email, PasswordHash, Salt, id_role_utilisateur)
            VALUES (@Nom, @Prenom, @Email, @PasswordHash, @Salt, @IdRole)";
            cmdInsert.Parameters.AddWithValue("@Nom", nom);
            cmdInsert.Parameters.AddWithValue("@Prenom", prenom);
            cmdInsert.Parameters.AddWithValue("@Email", email);
            cmdInsert.Parameters.AddWithValue("@PasswordHash", hash);
            cmdInsert.Parameters.AddWithValue("@Salt", salt);
            cmdInsert.Parameters.AddWithValue("@IdRole", roleId);

            cmdInsert.ExecuteNonQuery();
        }

        db.Close();
    }

        public static void DeleteUtilisateur(string email)
        {
            var db = new DatabaseConnexion(ConnectionString);
            db.Open();

            using(var cmd = db.CreateCommand())
            {
                cmd.CommandText = @" DELETE FROM Utilisateur WHERE Email = @Email;";
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.ExecuteNonQuery();

            }
        }



        public override string ToString()
        {
            return $"{Nom} {Prenom} ({Email}) - Rôle: {Role.Nom}";
        }
    }
}
