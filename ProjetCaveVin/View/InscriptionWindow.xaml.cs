using ProjetCaveVin.Model.Tables;
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
        public InscriptionWindow(Utilisateur currentUser)
        {
            InitializeComponent();
            DataContext = new InscriptionWindow(currentUser);
        }

        private void InscriptionButton_Click(object sender, RoutedEventArgs e)
        {
            var registrationWindow = new InscriptionWindow(currentUser: null);
            registrationWindow.Show();
        }
    }
}
