using System;
using System.Collections.Generic;
using ProjetCaveVin.Model.Connexion;
using ProjetCaveVin.Helpers;

namespace ProjetCaveVin.Model.Tables
{
    public class RoleAccess
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }

        private static string ConnectionString =>
            @"Server=localhost\SQLEXPRESS;Database=Cave;Trusted_Connection=True;Encrypt=False;";

        private static string ConnectionStringLocal => @"Server=172.16.119.42\SQLEXPRESS02,1433;Database=Cave;User Id=yohan;Password=1234;Encrypt=False;";


        public static RoleAccess GetByRole(string roleName)
        {
            var db = new DatabaseConnexion(ConnectionString);
            db.Open();

            using var cmd = db.CreateCommand();
            cmd.CommandText = "SELECT id_roleaccess, role_name, password_hash, salt FROM RoleAccess WHERE role_name = @r";
            cmd.Parameters.AddWithValue("@r", roleName);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new RoleAccess
                {
                    Id = Convert.ToInt32(reader["id_roleaccess"]),
                    RoleName = reader.GetString(reader.GetOrdinal("role_name")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("password_hash")),
                    Salt = reader.GetString(reader.GetOrdinal("salt"))
                };
            }

            return null;
        }

        public static void CreateOrInsertRole(string roleName, string plainPassword)
        {
            var (hash, salt) = PasswordHelper.HashPassword(plainPassword);

             var db = new DatabaseConnexion(ConnectionString);
            db.Open();

            using var cmd = db.CreateCommand();
            cmd.CommandText = "INSERT INTO RoleAccess (role_name, password_hash, salt) VALUES (@r, @h, @s)";
            cmd.Parameters.AddWithValue("@r", roleName);
            cmd.Parameters.AddWithValue("@h", hash);
            cmd.Parameters.AddWithValue("@s", salt);

            cmd.ExecuteNonQuery();
        }

        public static bool CheckPassword(string roleName, string password)
        {
             var db = new DatabaseConnexion(ConnectionString);
            db.Open();

            using var cmd = db.CreateCommand();
            cmd.CommandText = "SELECT password_hash, salt FROM RoleAccess WHERE role_name = @r";
            cmd.Parameters.AddWithValue("@r", roleName);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
                return false;

            string hash = reader.GetString(reader.GetOrdinal("password_hash"));
            string salt = reader.GetString(reader.GetOrdinal("salt"));

            return PasswordHelper.VerifyPassword(password, hash, salt);
        }

        public static List<RoleAccess> GetAllRoles()
        {
            var roles = new List<RoleAccess>();

             var db = new DatabaseConnexion(ConnectionString);
            db.Open();

            using var cmd = db.CreateCommand();
            cmd.CommandText = "SELECT id_roleaccess, role_name, password_hash, salt FROM RoleAccess";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                roles.Add(new RoleAccess
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id_roleaccess")),
                    RoleName = reader.GetString(reader.GetOrdinal("role_name")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("password_hash")),
                    Salt = reader.GetString(reader.GetOrdinal("salt"))
                });
            }

            return roles;
        }

        public static void InsertRoleAccess(string roleName, string password)
        {
            // string connectionString = @"Server=localhost\SQLEXPRESS;Database=Cave;Trusted_Connection=True;Encrypt=False;";
            var db = new DatabaseConnexion(ConnectionString);
            db.Open();
            try
            {
                var (hash, salt) = PasswordHelper.HashPassword(password);
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO RoleAccess (role_name, password_hash, salt) VALUES (@r, @h, @s)";
                    cmd.Parameters.AddWithValue("@r", roleName);
                    cmd.Parameters.AddWithValue("@h", hash);
                    cmd.Parameters.AddWithValue("@s", salt);
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                db.Close();
            }
        }

    }
}
