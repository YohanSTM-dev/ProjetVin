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

        public GestionBouteillePage()
        {
            InitializeComponent();
            _viewModel = new GestionBouteillePageViewModel();
            this.DataContext = _viewModel;
        }

        private void Recharger_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.RechargerBouteilles();
            LibelleTextBox.Text = string.Empty;
        }

        private void Zone_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is Zone zone)
            {
                _viewModel.FiltrerParZone(zone.Code);
            }
        }

        private void LibelleTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _viewModel.FiltrerParLibelle(LibelleTextBox.Text);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ReinitialiserFiltres();
            LibelleTextBox.Text = string.Empty;
        }
    }
}