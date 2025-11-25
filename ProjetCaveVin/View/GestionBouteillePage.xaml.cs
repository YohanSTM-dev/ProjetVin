using System.Windows;
using System.Windows.Controls;
using ProjetCaveVin.ViewModel;

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
        }
    }
}
