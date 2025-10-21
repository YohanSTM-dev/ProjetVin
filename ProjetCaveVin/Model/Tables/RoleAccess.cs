using System;
using System.Collections.Generic;
using ProjetCaveVin.Model.Connexion;
using ProjetCaveVin.Model.Tables;
using ProjetCaveVin.Helpers;



namespace ProjetCaveVin.Model.Tables;

using System.Threading.Tasks;
using ProjetCaveVin.Model.Connexion;

public class RoleAccess
{
    public int Id { get; set; }
    public string RoleName { get; set; }
    public string PasswordHash { get; set; }
    public string Salt { get; set; }

    public static RoleAccess GetByRole(string roleName)
    {
        string connectionString = @"Server=localhost\SQLEXPRESS;Database=Cave;Trusted_Connection=True;Encrypt=False;";

        var db = new DatabaseConnexion(connectionString);
        db.Open();
        using (var cmd = db.CreateCommand())
        {
            cmd.CommandText = "SELECT id_roleaccess, role_name, password_hash, salt FROM RoleAccess WHERE role_name = @r";
            cmd.Parameters.AddWithValue("@r", roleName);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new RoleAccess
                    {
                        Id = reader.GetInt32(0),
                        RoleName = reader.GetString(1),
                        PasswordHash = reader.GetString(2),
                        Salt = reader.GetString(3)
                    };
                }
            }
        }
        db.Close();
        return null;
    }

    public static void CreateRoleAccess(string roleName, string plainPassword)
    {
        string connectionString = @"Server=localhost\SQLEXPRESS;Database=Cave;Trusted_Connection=True;Encrypt=False;";

        var (hash, salt) = PasswordHelper.HashPassword(plainPassword);
        var db = new DatabaseConnexion(connectionString);
        db.Open();
        using (var cmd = db.CreateCommand())
        {
            cmd.CommandText = "INSERT INTO RoleAccess (role_name, password_hash, salt) VALUES (@r, @h, @s)";
            cmd.Parameters.AddWithValue("@r", roleName);
            cmd.Parameters.AddWithValue("@h", hash);
            cmd.Parameters.AddWithValue("@s", salt);
            cmd.ExecuteNonQuery();
        }
        db.Close();
    }

    public static bool CheckPassword(string nomRole, string password)
    {
        string connectionString = @"Server=localhost\SQLEXPRESS;Database=Cave;Trusted_Connection=True;Encrypt=False;";
        var db = new DatabaseConnexion(connectionString);
        db.Open();

        using (var cmd = db.CreateCommand())
        {
            cmd.CommandText = "SELECT PasswordHash, Salt FROM RoleAccess WHERE NomRole = @nom";
            cmd.Parameters.AddWithValue("@nom", nomRole);
            using (var reader = cmd.ExecuteReader())
            {
                if (!reader.Read())
                    return false;

                string hash = reader.GetString(0);
                string salt = reader.GetString(1);

                return PasswordHelper.VerifyPassword(password, hash, salt);
            }
        }
    }

    public static void InsertRoleAccess(string roleName, string password)
    {
        string connectionString = @"Server=localhost\SQLEXPRESS;Database=Cave;Trusted_Connection=True;Encrypt=False;";
        var db = new DatabaseConnexion(connectionString);
        db.Open();

        try
        {
            // Générer hash + salt
            var (hash, salt) = PasswordHelper.HashPassword(password);

            using (var cmd = db.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO RoleAccess (role_name, password_hash, salt) VALUES (@r, @h, @s)";
                cmd.Parameters.AddWithValue("@r", roleName);
                cmd.Parameters.AddWithValue("@h", hash);
                cmd.Parameters.AddWithValue("@s", salt);

                cmd.ExecuteNonQuery(); // Important : exécute la requête
            }
        }
        finally
        {
            db.Close(); // Toujours fermer la connexion
        }
    }

}

