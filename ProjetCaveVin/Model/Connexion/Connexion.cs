using System;

namespace ProjetCaveVin.Model.Connexion
{
    public class Connexion
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }          // Exemple : "172.16.119.42"
        public string Database { get; set; }      // Exemple : "restaurant"
        public string Instance { get; set; }      // Exemple : "SQLEXPRESS02"
        public int Port { get; set; } = 1433;     // Port par défaut SQL Server

        public Connexion() { }

        public Connexion(string username, string password, string host, string database, string instance = "SQLEXPRESS02", int port = 1433)
        {
            Username = username;
            Password = password;
            Host = host;
            Database = database;
            Instance = instance;
            Port = port;
        }

        public override string ToString()
        {
            // Chaîne complète de connexion SQL Server
            string serverPart = string.IsNullOrEmpty(Instance) ? Host : $"{Host}\\{Instance}";
            if (Port != 1433)
                serverPart += $",{Port}";

            return $"Server={serverPart};Database={Database};User Id={Username};Password={Password};Encrypt=False;";
        }
    }
}
