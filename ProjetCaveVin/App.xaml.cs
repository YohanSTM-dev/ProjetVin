using ProjetCaveVin.Model.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;

namespace ProjetCaveVin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Stocke la chaîne de connexion globale pour toute l'application
        public static string ConnectionString { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                //. Lire la variable d'environnement DB_CONNECTION
                ConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");
                if (string.IsNullOrEmpty(ConnectionString))
                {
                    throw new Exception("La variable d'environnement 'DB_CONNECTION' n'est pas définie.");
                }


                // . Initialiser les rôles si la table est vide
                List<RoleAccess> rolesExistants = RoleAccess.GetAllRoles();
                if (rolesExistants.Count == 0)
                {
                    RoleAccess.CreateOrInsertRole("Administrateur", "1234");
                    RoleAccess.CreateOrInsertRole("Sommelier", "1234");
                    RoleAccess.CreateOrInsertRole("Serveur", "1234");
                }
            }
            catch (Exception ex)
            {
                // Affiche l'erreur et empêche le crash brutal
                MessageBox.Show($"Erreur lors de l'initialisation de la base de données : \n{ex.Message}", "Erreur Système");
            }
        }
    }
}
