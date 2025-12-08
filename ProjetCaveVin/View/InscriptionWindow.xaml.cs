using System.Windows;
//using ProjetCaveVin.Model.Tables;
using ProjetCaveVin.ViewModel;
using ProjetCaveVin.Model.Classes;


namespace ProjetCaveVin.View
{
    public partial class InscriptionWindow : Window
    {
        private string _selectedRole;

        public InscriptionWindow(string selectedRole)
        {
            InitializeComponent();
            _selectedRole = selectedRole;
            DataContext = new InscriptionViewModel(selectedRole);
        }

        private void InscriptionButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is InscriptionViewModel vm)
            {
                vm.Password = PasswordBox.Password; 
                vm.InscriptionCommand.Execute(null);
            }
        }

        private void RetourButton_Click(object sender, RoutedEventArgs e)
        {
            Window targetWindow;

            switch (_selectedRole?.Trim().ToLower())
            {
                case "Administrateur":
                    targetWindow = new LoginAdminWindow();
                    break;
                case "sommelier":
                case "serveur":
                    targetWindow = new RoleAccessWindow();
                    break;
                default:
                    targetWindow = new LoginAdminWindow();
                    break;
            }

            targetWindow.Show();
            this.Close();
        }

    }
}
