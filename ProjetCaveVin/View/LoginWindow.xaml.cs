using System.Windows;
using ProjetCaveVin.Model.Tables;

namespace ProjetCaveVin.View
{
    public partial class LoginWindow : Window
    {
        private string _selectedRole;

        public LoginWindow(string selectedRole)
        {
            InitializeComponent();
            _selectedRole = selectedRole;

            System.Diagnostics.Debug.WriteLine($"LoginWindow ouvert avec _selectedRole = {_selectedRole}");
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text;
            string password = PasswordBox.Password;

            var user = Utilisateur.GetByCredentials(email, password);

            if (user == null)
            {
                MessageText.Text = "Email ou mot de passe incorrect.";
                return;
            }

            if (user.Role.Nom != _selectedRole)
            {
                MessageText.Text = $"Le rôle ne correspond pas au rôle choisi : {_selectedRole}.";
                return;
            }

            Window nextWindow = user.Role.Nom switch
            {
                "Administrateur" => new AdministrateurWindow(),
                "Sommelier" => new SommelierWindow(),
                "Serveur" => new ServeurWindow(),
                _ => null
            };

            nextWindow?.Show();
            this.Close();
        }

        private void RetourButton_Click(object sender, RoutedEventArgs e)
        {
            Window targetWindow;

            switch (_selectedRole?.Trim().ToLower())
            {
                case "administrateur":
                    targetWindow = new LoginAdminWindow();
                    break;
                case "sommelier":
                case "serveur":
                    targetWindow = new RoleAccessWindow();
                    break;
                default:
                    targetWindow = new LoginWindow(_selectedRole); 
                    break;
            }

            targetWindow.Show();
            this.Close();
        }
    }
}
