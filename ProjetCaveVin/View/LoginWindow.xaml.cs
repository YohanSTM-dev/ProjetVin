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

            // Affiche le rôle choisi
            //SelectedRoleText.Text = $"Vous vous connectez en tant que : {_selectedRole}";
        }

        public LoginWindow()
        {
            InitializeComponent();

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

            // Ouvre la bonne fenêtre selon le rôle
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
    }
}
