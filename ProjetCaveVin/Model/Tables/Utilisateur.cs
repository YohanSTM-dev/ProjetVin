using ProjetCaveVin.Model.Connexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCaveVin.Model.Tables
{
    public class Utilisateur
    {

        public int id_utilisateur { get; set; }
        public string Nom { get; set; }

        public string Prenom { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        

        public Utilisateur() { }

        public Utilisateur(int id, string name, string lastname, string email, string password, string role)
        {
            id_utilisateur = id;
            Nom = name;
            Prenom = lastname;
            Email = email;
            Password = password;
            this.Role = role;
        }

        public static List<Utilisateur> GetAllUtilisateur()
        {
            var Utilisateurs = new List<Utilisateur>();
            string connectionString = @"Server=172.16.119.42\SQLEXPRESS02,1433;Database=restaurant;User Id=yohan;Password=1234;Encrypt=False;";
            var Database = new DatabaseConnexion(connectionString);
            Database.Open();
            using (var command = Database.CreateCommand())
            {
                command.CommandText = "select id_utilisateur,Nom,Prenom,Role,Email,Password from Utilisateur";
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
                            Password = reader.GetString(4),
                            Role = reader.GetString(5)
                        };
                        Utilisateurs.Add(utilisateur);

                    }


                }


            }

            Database.Close();
            return Utilisateurs;


        }

        public static Utilisateur GetByCredentials(string email, string password)
        {

            string req = "select id_utilisateur, Nom,Prenom,Role, Email, Password from Utilisateur where Email = @Email AND Password = @Password;" ;
            var Utilisateurs = new List<Utilisateur>();
            string connectionString = @"Server=172.16.119.42\SQLEXPRESS02,1433;Database=Cave;User Id=yohan;Password=1234;Encrypt=False;";
            var db = new DatabaseConnexion(connectionString);
            db.Open();
            using(var command = db.CreateCommand())
            {
                command.CommandText = req;
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);

                using(var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Utilisateur
                        {
                            id_utilisateur = reader.GetInt32(0),
                            Nom = reader.GetString(1),
                            Prenom = reader.GetString(2),
                            Role = reader.GetString(3),
                            Email = reader.GetString(4),
                            Password = reader.GetString(5)
                           
                        };
                        
                    }
                }

            }
            db.Close();
            return null;
        }

        public override string ToString()
        {
            return base.ToString();

        }
    }
}
