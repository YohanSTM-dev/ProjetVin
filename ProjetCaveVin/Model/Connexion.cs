using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks

namespace ProjetCaveVin.Model
{
    public class Connexion
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int Port { get; set; } = 1433;
        public string Host { get; set; }
        public string Database { get; set; }
        public Connexion() { }

        public Connexion(string username, string password, int port, string host, string database)
        {
            Username = username;
            Password = password;
            Port = port;
            Host = host;
            Database = database;
            SqlConnexion dl = new SqlConnexion(this);
        }

        public override string ToString()
        {
            return $"Server={Host};Port={Port};Database={Database};User Id={Username};Password={Password};";
        }

    }
}
