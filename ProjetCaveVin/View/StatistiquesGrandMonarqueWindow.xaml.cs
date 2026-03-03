using ProjetCaveVin.Helpers;
using ProjetCaveVin.Model.Classes;
using ProjetCaveVin.Model.Connexion;
//using ProjetCaveVin.Model.Tables;
using ProjetCaveVin.ViewModel;
using System.Windows;
using System.Windows.Controls;



namespace ProjetCaveVin.View
{
    /// <summary>
    /// Logique d'interaction pour StatistiquesGrandMonarqueWindow.xaml
    /// </summary>
    public partial class StatistiquesGrandMonarqueWindow : Window
    {
        StatistiquesGrandMonarqueViewModel _dataContext = new();
        public StatistiquesGrandMonarqueWindow()
        {
            InitializeComponent();
            _dataContext = new StatistiquesGrandMonarqueViewModel();
            DataContext = _dataContext;
        }
        private void RetourButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
      
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}