using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using ProjetCaveVin.Model.Classes;
using ProjetCaveVin.ViewModel;


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

            var viewModel = new GestionBouteilleWindowViewModel();

            var window = new GestionBouteilleWindow();
            window.Show();

            this.Close();
        }

        private void Button_Test_Connexion_Utilisateur(object sender, RoutedEventArgs e)
        {

        }

        private void ImageStat_Click(object sender, MouseButtonEventArgs e)
        {
            var page = new StatistiquesGrandMonarqueWindow();

            page.Show();
            this.Close();
        }
        private void RetourButton_Click(object sender, RoutedEventArgs e)
        {
            var roleAccessWindow = new RoleAccessWindow();
            roleAccessWindow.Show();
            this.Close();
        }
    }
}
