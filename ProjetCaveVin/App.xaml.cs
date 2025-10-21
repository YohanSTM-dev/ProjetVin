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

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string dbc = @"Server=localhost\SQLEXPRESS;Database=Cave;Trusted_Connection=True;Encrypt=False;";
            var conn = new DatabaseConnexion(dbc);
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                
                cmd.CommandText = "select Count(*) from RoleAccess;";
                long count = (long)cmd.ExecuteScalar();

                if(count == 0)
                {
                    RoleAccess.InsertRoleAccess("Serveur", "serveur123");
                    RoleAccess.InsertRoleAccess("Sommelier", "sommelier123");
                    RoleAccess.InsertRoleAccess("Administrateur", "admin123");
                }

            }

            var roleAcessWindow = new RoleAccessWindow();
            roleAcessWindow.Show();
        }
    }

}
