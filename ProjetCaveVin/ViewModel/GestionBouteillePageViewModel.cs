using ProjetCaveVin.Model.Classes;
using ProjetCaveVin.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProjetCaveVin.ViewModel
{
    public class GestionBouteillePageViewModel : INotifyPropertyChanged
    {
        private GestionBouteille _gestionBouteille;
        private GestionEmplacementViewModel _gestionEmplacementViewModel;

        public ObservableCollection<Bouteille> Bouteilles
        {
            get => _gestionBouteille.Bouteilles;
        }
        public ObservableCollection<Zone> Zones
        {
            get => _gestionEmplacementViewModel.Zones;
        }
        public GestionBouteillePageViewModel()
        {
            _gestionBouteille = new GestionBouteille();
            _gestionEmplacementViewModel = new GestionEmplacementViewModel();
        }
        




        public void RechargerBouteilles()
        {
            _gestionBouteille = new GestionBouteille();
            OnPropertyChanged(nameof(Bouteilles));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
