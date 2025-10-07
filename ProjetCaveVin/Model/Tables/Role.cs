using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetCaveVin.Model.Connexion;


namespace ProjetCaveVin.Model.Tables
{
    public class Role
    {

        public int id_role { get; set; }
        public string Nom { get; set; }

        public Role() { }

        public Role(int id, string name)
        {
            id_role = id;
            Nom = name;
        }

        public static List<Role> GetAllRole()
        {

            var roles = new List<Role>();
            string connectionString = @"Server=172.16.119.42\SQLEXPRESS02,1433;Database=Cave;User Id=yohan;Password=1234;Encrypt=False;";
            var db = new DatabaseConnexion(connectionString);
            db.Open();

            using (var command = db.CreateCommand())
            {
                command.CommandText = "SELECT id_role, Nom FROM Role";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        roles.Add(new Role
                        {
                            id_role = reader.GetInt32(0),
                            Nom = reader.GetString(1)
                        });

                    }
                }
            }

            db.Close();
            return roles;
        }

        public static Role GetById(int id)
        {
            string connectionString = @"Server=172.16.119.42\SQLEXPRESS02,1433;Database=Cave;User Id=yohan;Password=1234;Encrypt=False;";
            var db = new DatabaseConnexion(connectionString);
            db.Open();

            using (var command = db.CreateCommand())
            {
                command.CommandText = "SELECT id_role, nom FROM Role WHERE id_role = @Id";
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Role
                        {
                            id_role = reader.GetInt32(0),
                            Nom = reader.GetString(1)
                        };
                    }
                }
            }

            db.Close();
            return null;
        }

        public static Role GetByName(string nom)
        {
            string connectionString = @"Server=172.16.119.42\SQLEXPRESS02,1433;Database=Cave;User Id=yohan;Password=1234;Encrypt=False;";
            var db = new DatabaseConnexion(connectionString);
            db.Open();

            using (var command = db.CreateCommand())
            {
                command.CommandText = "SELECT id_role, nom FROM Role WHERE nom = @Nom";
                command.Parameters.AddWithValue("@Nom", nom);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Role
                        {
                            id_role = reader.GetInt32(0),
                            Nom = reader.GetString(1)
                        };
                    }
                }
            }

            db.Close();
            return null;
        }
    }
}
