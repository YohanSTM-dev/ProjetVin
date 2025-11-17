using System.Windows;
using System.Windows.Controls;
using ProjetCaveVin.Model.Tables;
using ProjetCaveVin.Model.Classes;

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
            _selectedRole = (string)((Button)sender).Tag;
            SelectedRoleText.Text = $"Rôle sélectionné : {_selectedRole}";

            PasswordPanel.Visibility = Visibility.Visible;

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

            if (RoleAccess.CheckPassword(_selectedRole, password))
            {
                if (_selectedRole == "Administrateur")
                {
                    new LoginAdminWindow().Show();
                }
                else
                {
                    new LoginWindow("Administrateur").Show();
                }

                this.Close(); 
            }
            else
            {
                MessageText.Text = "Mot de passe incorrect.";
            }
        }
    }
}
