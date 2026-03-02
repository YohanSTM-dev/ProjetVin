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
<<<<<<< HEAD


        public void RetourButton_Click(object sender, RoutedEventArgs e)
        {
            Window MainWindow = new MainWindow();
            MainWindow.Show();
=======
        private void RetourButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
>>>>>>> e68fe6860d3a3d5431aacb4ab4561d27309535c9
            this.Close();
        }
    }
}