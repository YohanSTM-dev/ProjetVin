using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using ProjetCaveVin.Model.Classes;


namespace ProjetCaveVin.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void ImageGestionVin_Click(object sender, MouseButtonEventArgs e)
        {
            var page = new GestionBouteillePage();
            var navWindow = new NavigationWindow
            {
                Content = page,
                Title = "Gestion des Bouteilles"
            };
            navWindow.Show();
            this.Close();
        }

        private void Button_Test_Connexion_Utilisateur(object sender, RoutedEventArgs e)
        {

        }
    }
}
