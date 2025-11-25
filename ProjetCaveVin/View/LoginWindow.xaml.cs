using System.Windows;
//using ProjetCaveVin.Model.Tables;
using ProjetCaveVin.ViewModel;
using ProjetCaveVin.View;
using System.Windows.Controls;
using ProjetCaveVin.Model.Classes;


namespace ProjetCaveVin.View
{
    public partial class LoginWindow : Window
    {
        private string _selectedRole;

        public LoginWindow(string selectedRole)
        {
            InitializeComponent();
            _selectedRole = selectedRole;

            var vm = new LoginViewModel();
            vm.LoginSucceeded += OnLoginSucceeded;
            DataContext = vm;
        }

        private void OnLoginSucceeded(Utilisateur user)
        {
            if (user.Role.Nom != _selectedRole)
            {
                MessageBox.Show($"Le rôle ne correspond pas au rôle choisi : {_selectedRole}.",
                                "Erreur de rôle", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Window nextWindow = null  ;
            
            switch(user.Role.Nom)
            {
                case "Administrateur":
                    nextWindow = new MainWindow();
                    break;
                case "Serveur":
                    nextWindow = new ServeurWindow();
                    break;
                case "Sommelier":
                    nextWindow = new SommelierWindow();
                    break;
                default:
                    nextWindow = null;
                    break;  
            } 

            nextWindow?.Show();
            this.Close();
        }

        private void RetourButton_Click(object sender, RoutedEventArgs e)
        {
            Window targetWindow = _selectedRole?.Trim().ToLower() switch
            {
                "administrateur" => new LoginAdminWindow(),
                "sommelier" or "serveur" => new RoleAccessWindow(),
                _ => new RoleAccessWindow()
            };

            targetWindow.Show();
            this.Close();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.Password = (sender as PasswordBox)?.Password;
            }
        }

    }
}
