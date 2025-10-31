using System.Windows;
using ProjetCaveVin.Model.Tables;
using ProjetCaveVin.ViewModel;

namespace ProjetCaveVin.View
{
    /// <summary>
    /// Logique d'interaction pour GestionBouteilleWindow.xaml
    /// </summary>
    public partial class GestionBouteilleWindow : Window
    {
        public GestionBouteilleWindow()
        {
            InitializeComponent();
            DataContext = new GestionBouteilleViewModel();
        }


    }
}