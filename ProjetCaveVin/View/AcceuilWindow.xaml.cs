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
    /// Logique d'interaction pour AcceuilWindow.xaml
    /// </summary>
    public partial class AcceuilWindow : Window
    {
        public AcceuilWindow()
        {
            InitializeComponent();
        }

        private void BtnSeConnecter_Click(object sender, RoutedEventArgs e)
        {
            var login = new LoginWindow();
            login.Show();
            this.Close();
        }

        private void BtnSInscrire_Click(object sender, RoutedEventArgs e)
        {
            var Inscription  = new InscriptionWindow();
            Inscription.Show();
            this.Close();
        }
    }
}
