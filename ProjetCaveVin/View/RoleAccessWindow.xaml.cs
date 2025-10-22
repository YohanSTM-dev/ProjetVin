using System.Windows;
using System.Windows.Controls;
using ProjetCaveVin.Model.Tables;
using ProjetCaveVin.View;

namespace ProjetCaveVin.View
{
    public partial class RoleAccessWindow : Window
    {
        private string _selectedRole;

        public RoleAccessWindow()
        {
            InitializeComponent();
        }

        private void RoleButton_Click(object sender, RoutedEventArgs e)
        {
            // Récupère le rôle depuis le Tag du bouton
            _selectedRole = (string)((Button)sender).Tag;
            SelectedRoleText.Text = $"Rôle sélectionné : {_selectedRole}";

            // Affiche le panel pour entrer le mot de passe
            PasswordPanel.Visibility = Visibility.Visible;

            // Reset
            RolePasswordBox.Password = "";
            MessageText.Text = "";
        }

        private void ValidateRoleAccess_Click(object sender, RoutedEventArgs e)
        {
            string password = RolePasswordBox.Password;

            if (string.IsNullOrEmpty(password))
            {
                MessageText.Text = "Veuillez entrer le mot de passe.";
                return;
            }

            // Vérifie le mot de passe dans la table RoleAccess
            if (RoleAccess.CheckPassword(_selectedRole, password))
            {
                // Ouvre la fenêtre correspondante
                if (_selectedRole == "Administrateur")
                {
                    new LoginAdminWindow().Show();
                }
                else
                {
                    new LoginWindow().Show();
                }

                this.Close(); // Ferme la fenêtre actuelle
            }
            else
            {
                MessageText.Text = "Mot de passe incorrect.";
            }
        }
    }
}
