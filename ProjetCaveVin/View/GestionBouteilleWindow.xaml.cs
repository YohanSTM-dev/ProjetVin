using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ProjetCaveVin.ViewModel;
using ProjetCaveVin.Model.Classes;



namespace ProjetCaveVin.View
{
    public partial class GestionBouteilleWindow : Window
    {
        private GestionBouteilleWindowViewModel _dataContext;
        public GestionBouteilleWindow()
        {
            InitializeComponent();
            _dataContext = new GestionBouteilleWindowViewModel();
            this.DataContext = _dataContext; // ← essentiel pour que le XAML voie Bouteilles et Zones
        }

        private void Recharger_Click(object sender, RoutedEventArgs e)
        {
            _dataContext.RechargerBouteilles();
            LibelleTextBox.Text = string.Empty;
        }

        private void Zone_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is Zone zone)
            {
                _dataContext.FiltrerParZone(zone.Code);
            }
        }

        private void LibelleTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _dataContext.FiltrerParLibelle(LibelleTextBox.Text);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            _dataContext.ReinitialiserFiltres();
            LibelleTextBox.Text = string.Empty;
        }
        private void RetourButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}