using ProjetCaveVin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetCaveVin.Model;
using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ProjetCaveVin.Helpers
{
    public static class DatabaseConnexion
    {

        public static bool TestConnexion(Connexion c)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(c.ToString()))
                {
                    connection.Open();
                    Console.Write("Connexion réussie");
                    return true;
                }

            }
            catch (Exception ex)
            {
                Console.Write($"Erreur de connexion: {ex.Message}");
                return false;
            }
        }
    }
}
     