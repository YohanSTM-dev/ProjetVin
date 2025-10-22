using ProjetCaveVin.Model.Tables;
using ProjetCaveVin.ViewModel;
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

namespace ProjetCaveVin.View
{
    /// <summary>
    /// Logique d'interaction pour InscriptionWindow.xaml
    /// </summary>
    public partial class InscriptionWindow : Window
    {

        private Utilisateur _currentUser;
        public InscriptionWindow(Utilisateur currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            DataContext = new InscriptionViewModels(currentUser);
        }


        public InscriptionWindow()  :this(null)
        {

        }


        private void InscriptionButton_Click(object sender, RoutedEventArgs e)
        {
            var registrationWindow = new InscriptionWindow(currentUser: null);
            registrationWindow.Show();
        }

        private void RetourButton_Click(object sender, RoutedEventArgs e)
        {
/*            var acceuilWindow = new AcceuilWindow();
            acceuilWindow.Show();
            this.Close();*/
        }
    }
}
