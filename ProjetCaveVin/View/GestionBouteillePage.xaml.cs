using System.Windows;
using System.Windows.Controls;
using ProjetCaveVin.Model;

namespace ProjetCaveVin.View
{
    public partial class GestionBouteillePage : Page
    {
        private GestionBouteille _gestionBouteille;

        public GestionBouteillePage()
        {
            InitializeComponent();
            _gestionBouteille = new GestionBouteille();
            this.DataContext = _gestionBouteille;
        }

        private void Recharger_Click(object sender, RoutedEventArgs e)
        {
            // Recharge la liste
            _gestionBouteille = new GestionBouteille();
            this.DataContext = _gestionBouteille;
        }
    }
}
