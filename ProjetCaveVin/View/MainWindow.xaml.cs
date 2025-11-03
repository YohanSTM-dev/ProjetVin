using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

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
    }
}
