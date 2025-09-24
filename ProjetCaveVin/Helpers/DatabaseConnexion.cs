using ProjetCaveVin.Model;
using System;
using System.Data; 
using Microsoft.Data.SqlClient;
using System.Data.SqlClient;

namespace ProjetCaveVin.Helpers
{
    public class DatabaseConnexion 
    {
        private SqlConnection _connection;

        public DatabaseConnexion(Connexion connexion)
        {
            _connection = new SqlConnection(connexion.ToString());
        }

        public DatabaseConnexion(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public void Open()
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
        }

        public bool IsOpenConnected()
        {
            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problème de connexion : " + ex.Message);
                return false;
            }
        }

        public void Close()
        {
            if (_connection.State != ConnectionState.Closed)
                _connection.Close();
        }
    }
}
