using System.Windows;
using ProjetCaveVin.Helpers;

namespace ProjetCaveVin.View
{
    public partial class RoleAccessWindow : Window
    {
        private string _selectedRole;
        private int _attempts = 0;
        private const int MAX_ATTEMPTS = 5;

        public RoleAccessWindow()
        {
            InitializeComponent();
        }

        private void RoleButton_Click(object sender, RoutedEventArgs e)
        {
            _selectedRole = (string)((FrameworkElement)sender).Tag;
            SelectedRoleText.Text = $"Rôle sélectionné : {_selectedRole}";
            PasswordPanel.Visibility = Visibility.Visible;
            MessageText.Text = "";
            RolePasswordBox.Password = "";
        }

        private void ValidateRoleAccess_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedRole))
            {
                MessageText.Text = "Choisissez un rôle d'abord.";
                return;
            }

            var pass = RolePasswordBox.Password;

            if (RoleAccessHelper.CheckRolePassword(_selectedRole, pass))
            {
                // Ouvre la fenêtre de login avec le rôle choisi
                var loginWindow = new LoginWindow(_selectedRole);
                loginWindow.Show();
                this.Close();
                return;
            }

            _attempts++;
            MessageText.Text = "Mot de passe rôle incorrect.";
            if (_attempts >= MAX_ATTEMPTS)
            {
                MessageText.Text = "Trop d'essais. Contactez l'administrateur.";
                RolePasswordBox.IsEnabled = false;
            }
        }
    }
}
