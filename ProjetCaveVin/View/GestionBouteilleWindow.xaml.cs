using System.Windows;
using ProjetCaveVin.Model.Tables;
using ProjetCaveVin.ViewModel;
using ProjetCaveVin.Model.Classes;


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