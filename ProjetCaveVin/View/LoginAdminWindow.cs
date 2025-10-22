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
using ProjetCaveVin.View;
using ProjetCaveVin.ViewModel;
using ProjetCaveVin.Model.Tables;


namespace ProjetCaveVin.View
{
    /// <summary>
    /// Logique d'interaction pour AcceuilWindow.xaml
    /// </summary>
    public partial class LoginAdminWindow : Window
    {
        public LoginAdminWindow()
        {
            InitializeComponent();
            DataContext = new LoginAdminViewModel();
        }

        private void ConnexionButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow("Administrateur");
            loginWindow.Show();
            this.Close(); 
        }

        private void InscriptionButton_Click(object sender, RoutedEventArgs e)
        {
            var inscriptionWindow = new InscriptionWindow(null);
            inscriptionWindow.Show();
            this.Close();
        }

        private void RetourButton_Click(object sender, RoutedEventArgs e)
        {
            var roleAccessWindow = new RoleAccessWindow();
            roleAccessWindow.Show();
            this.Close();
        }


    }
}
