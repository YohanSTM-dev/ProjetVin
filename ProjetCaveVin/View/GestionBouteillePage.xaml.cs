using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ProjetCaveVin.ViewModel;
using ProjetCaveVin.Model.Classes;

namespace ProjetCaveVin.View
{
    public partial class GestionBouteillePage : Page
    {
        private GestionBouteillePageViewModel _viewModel;
        private string ZoneSelectionnee;

        public GestionBouteillePage()
        {
            InitializeComponent();
            _viewModel = new GestionBouteillePageViewModel();
            this.DataContext = _viewModel;
        }

        private void Recharger_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.RechargerBouteilles();
        }

        private void Zone_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is Zone zone)
            {
                ZoneSelectionnee = zone.Code;
                MessageBox.Show("Zone sélectionnée : " + ZoneSelectionnee);
            }
        }
    }
}
