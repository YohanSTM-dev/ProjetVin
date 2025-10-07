using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ProjetCaveVin.ViewModel;
using ProjetCaveVin.Model.Tables;

namespace ProjetCaveVin.View
{
    /// <summary>
    /// Logique d'interaction pour LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            var viewModel = new LoginViewModel();
            DataContext = viewModel;


            viewModel.LoginSucceeded += OnLoginSucceeded;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as LoginViewModel;
            if (viewModel != null)
            {
                viewModel.Password = PasswordBox.Password;
                viewModel.LoginCommand.Execute(null);
            }
        }

        private void OnLoginSucceeded(Utilisateur user)
        {
            if (user == null)
            {
                MessageBox.Show("Utilisateur non trouvé.");
                return;
            }

            if (user.Role == null)
            {
                MessageBox.Show("Rôle utilisateur introuvable.");
                return;
            }

            Window nextWindow = null;

            switch (user.Role.Nom?.ToLower())
            {
                case "administrateur":
                    nextWindow = new AdministrateurWindow();
                    break;
                case "serveur":
                    nextWindow = new ServeurWindow();
                    break;
                case "sommelier":
                    nextWindow = new SommelierWindow();
                    break;
                default:
                    MessageBox.Show($"Rôle inconnu : {user.Role.Nom}");
                    return;
            }
            nextWindow?.Show();
            this.Close();
        }

        private void InscriptionButton_Click(object sender, RoutedEventArgs e)
        {
            var inscriptionWindow = new InscriptionWindow(null); 
            inscriptionWindow.Show();
            this.Close();

        }
    }
}
 
