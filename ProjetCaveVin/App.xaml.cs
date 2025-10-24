using System.Configuration;
using System.Data;
using System.Windows;
using ProjetCaveVin.Model.Connexion;
using ProjetCaveVin.View;
using ProjetCaveVin.Model.Tables;

namespace ProjetCaveVin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        // fonctionnalité plus utile et gerer automatiquement avec la bd 

        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    base.OnStartup(e);

        //    try
        //    {

        //        string connectionString = @"Server=localhost\SQLEXPRESS;Database=Cave;Trusted_Connection=True;Encrypt=False;";
        //        var db = new DatabaseConnexion(connectionString);
        //        db.Open();


        //        using (var cmd = db.CreateCommand())
        //        {
        //            cmd.CommandText = "SELECT COUNT(*) FROM RoleAccess;";
        //            int count = Convert.ToInt32(cmd.ExecuteScalar()); 

        //            if (count == 0)
        //            {
        //                // Créer les rôles par défaut
        //                RoleAccess.InsertRoleAccess("Serveur", "serveur123");
        //                RoleAccess.InsertRoleAccess("Sommelier", "sommelier123");
        //                RoleAccess.InsertRoleAccess("Administrateur", "admin123");
        //            }
        //        }

        //        db.Close();

        //        // Ouvrir la fenêtre de sélection du rôle
        //        var roleAccessWindow = new RoleAccessWindow();
        //        roleAccessWindow.Show();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Erreur au démarrage : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        //        Shutdown(); // Ferme l'application si la base n'est pas accessible
        //    }
        //}

    }

}
