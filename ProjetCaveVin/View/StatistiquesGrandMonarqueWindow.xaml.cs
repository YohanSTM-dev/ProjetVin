using System.Windows;
//using ProjetCaveVin.Model.Tables;
using ProjetCaveVin.ViewModel;
using ProjetCaveVin.Model.Connexion;
using ProjetCaveVin.Helpers;
using ProjetCaveVin.Model.Classes;



namespace ProjetCaveVin.View
{
    /// <summary>
    /// Logique d'interaction pour StatistiquesGrandMonarqueWindow.xaml
    /// </summary>
    public partial class StatistiquesGrandMonarqueWindow : Window
    {
        public StatistiquesGrandMonarqueWindow()
        {
            InitializeComponent();
            DataContext = new StatistiquesGrandMonarqueViewModel();
        }


        public void RetourButton_Click(object sender, RoutedEventArgs e)
        {
            Window MainWindow = new MainWindow();
            MainWindow.Show();
            this.Close();
        }
    }
}