using ProjetCaveVin.Model.Classes;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows;

namespace ProjetCaveVin;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    // Cette méthode s'exécute tout au début, avant d'ouvrir la première fenêtre
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        try
        {
            // 1. On regarde si des rôles existent déjà
            List<RoleAccess> rolesExistants = RoleAccess.GetAllRoles();

            // 2. Si la liste est vide (ce qui est le cas après ton DELETE SQL)
            if (rolesExistants.Count == 0)
            {
                // 3. On crée les rôles avec un VRAI hash valide généré par le C#
                // Mot de passe par défaut : "1234"
                RoleAccess.CreateOrInsertRole("Administrateur", "1234");
                RoleAccess.CreateOrInsertRole("Sommelier", "1234");
                RoleAccess.CreateOrInsertRole("Serveur", "1234");
            }
        }
        catch (System.Exception ex)
        {
            // Si la base n'est pas accessible, on affiche l'erreur mais on ne crash pas tout de suite
            MessageBox.Show($"Erreur lors de l'initialisation de la base de données : \n{ex.Message}", "Erreur Système");
        }
    }
}