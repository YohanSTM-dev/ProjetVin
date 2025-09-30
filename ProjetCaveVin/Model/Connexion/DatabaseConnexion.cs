using ProjetCaveVin.Model;
using System;
using System.Data; 
using Microsoft.Data.SqlClient;
using System.Data.SqlClient;
using System.IO.Packaging;

namespace ProjetCaveVin.Model.Connexion
{
    public class DatabaseConnexion 
    {
        private SqlConnection _connection;
        public string CommandText { get; internal set; }

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

        public int Execute(string query)
        {
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                return command.ExecuteNonQuery();
            }
        }

        public SqlCommand createCommand()
        {
            return _connection.CreateCommand();
        }

        public SqlDataReader ExecuteReader(string query)
        {
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                return command.ExecuteReader();
            }
        }

        public string GetConnectionString()
        {
            return _connection.ConnectionString;
        }
    }

}
