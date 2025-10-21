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
        public Role Role { get; set; }

        public Utilisateur() { }

        public Utilisateur(int id, string nom, string prenom, string email, string passwordHash, Role role)
        {
            id_utilisateur = id;
            Nom = nom;
            Prenom = prenom;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
        }

        public static List<Utilisateur> GetAllUtilisateur()
        {
            var utilisateurs = new List<Utilisateur>();

            // string connectionString = @"Server=172.16.119.42\SQLEXPRESS02,1433;Database=Cave;User Id=yohan;Password=1234;Encrypt=False;";

            string connectionString = @"Server=localhost\SQLEXPRESS;Database=Cave;Trusted_Connection=True;Encrypt=False;";

            var db = new DatabaseConnexion(connectionString);
            db.Open();

            using (var command = db.CreateCommand())
            {
                command.CommandText = @"
                    SELECT u.id_utilisateur, u.Nom, u.Prenom, u.Email, u.PasswordHash, r.id_role, r.nom
                    FROM Utilisateur u
                    INNER JOIN Role r ON u.id_role_utilisateur = r.id_role
                ";

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
                            Role = new Role
                            {
                                id_role = reader.GetInt32(5),
                                Nom = reader.GetString(6)
                            }
                        };
                        utilisateurs.Add(utilisateur);
                    }
                }
            }

            db.Close();
            return utilisateurs;
        }
        public static Utilisateur GetByCredentials(string email, string password)
        {

            //  string connectionString = @"Server=172.16.119.42\SQLEXPRESS02,1433;Database=Cave;User Id=yohan;Password=1234;Encrypt=False;";

            string connectionString = @"Server=localhost\SQLEXPRESS;Database=Cave;Trusted_Connection=True;Encrypt=False;";

            var db = new DatabaseConnexion(connectionString);
            db.Open();

            try
            {
                using (var command = db.CreateCommand())
                {
                    command.CommandText = @"
                SELECT u.id_utilisateur, u.Nom, u.Prenom, u.Email, u.PasswordHash, r.id_role, r.nom
                FROM Utilisateur u
                INNER JOIN Role r ON u.id_role_utilisateur = r.id_role
                WHERE u.Email = @Email AND u.PasswordHash = @Password";

                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Utilisateur
                            {
                                id_utilisateur = reader.GetInt32(0),
                                Nom = reader.GetString(1),
                                Prenom = reader.GetString(2),
                                Email = reader.GetString(3),
                                PasswordHash = reader.GetString(4),
                                Role = new Role
                                {
                                    id_role = reader.GetInt32(5),
                                    Nom = reader.GetString(6)
                                }
                            };
                        }
                    }
                }
            }
            finally
            {
                db.Close();
            }

            return null; 
        }

        public static void Create(string nom, string prenom, string email, string password, string role)
        {
            string connectionString = @"Server=...;Database=Cave;User Id=...;Password=...;Encrypt=False;";
            var db = new DatabaseConnexion(connectionString);
            db.Open();

            string req = "INSERT INTO Utilisateur (Nom, Prenom, Email, PasswordHash, id_role_utilisateur) " +
                         "VALUES (@Nom, @Prenom, @Email, @Password, (SELECT id_role FROM Role WHERE nom = @Role))";

            using (var cmd = db.CreateCommand())
            {
                cmd.CommandText = req;
                cmd.Parameters.AddWithValue("@Nom", nom);
                cmd.Parameters.AddWithValue("@Prenom", prenom);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Role", role);

                cmd.ExecuteNonQuery();
            }

            db.Close();
        }

        public static void Add(Utilisateur user)
        {
            //string connectionString = @"Server=172.16.119.42\SQLEXPRESS02,1433;Database=Cave;User Id=yohan;Password=1234;Encrypt=False;";

            string connectionString = @"Server=localhost\SQLEXPRESS,1433;Database=Cave;Trusted_Connection=True;Encrypt=False;";

            var db = new DatabaseConnexion(connectionString);
            db.Open();

            using (var command = db.CreateCommand())
            {
                command.CommandText = @"INSERT INTO Utilisateur (Nom, Prenom, Email, PasswordHash, id_role_utilisateur)
                                VALUES (@Nom, @Prenom, @Email, @PasswordHash, @IdRole)";
                command.Parameters.AddWithValue("@Nom", user.Nom);
                command.Parameters.AddWithValue("@Prenom", user.Prenom);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                command.Parameters.AddWithValue("@IdRole", user.Role.id_role);

                command.ExecuteNonQuery();
            }

            db.Close();
        }




        public override string ToString()
        {
            return $"{Nom} {Prenom} ({Email}) - Role: {Role}";
        }
    }
}
