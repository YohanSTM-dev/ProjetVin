using ProjetCaveVin.Model.Classes;
using ProjetCaveVin.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace ProjetCaveVin.ViewModel
{
    public class GestionBouteillePageViewModel : INotifyPropertyChanged
    {
        private GestionBouteille _gestionBouteille;
        private GestionEmplacementViewModel _gestionEmplacementViewModel;

        // Filtres actifs
        private string _filtreZone;
        private string _filtreLibelle;

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
            _filtreZone = null;
            _filtreLibelle = null;
            OnPropertyChanged(nameof(Bouteilles));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void FiltrerParZone(string codeZone)
        {
            _filtreZone = codeZone;
            AppliquerFiltres();
        }

        public void FiltrerParLibelle(string libelle)
        {
            _filtreLibelle = libelle;
            AppliquerFiltres();
        }

        private void AppliquerFiltres()
        {
            // Partir de toutes les bouteilles ou des bouteilles de la zone
            var bouteillesFiltrees = string.IsNullOrWhiteSpace(_filtreZone)
                ? _gestionBouteille.AllBouteilles
                : _gestionEmplacementViewModel.GetBouteillesParZone(_filtreZone);

            // Appliquer le filtre sur le libellé si présent
            if (!string.IsNullOrWhiteSpace(_filtreLibelle))
            {
                bouteillesFiltrees = bouteillesFiltrees
                    .Where(b => b.Libelle.ToLower().Contains(_filtreLibelle.ToLower()))
                    .ToList();
            }

            _gestionBouteille.Bouteilles = new ObservableCollection<Bouteille>(bouteillesFiltrees);
            OnPropertyChanged(nameof(Bouteilles));
        }

        // Méthode pour réinitialiser tous les filtres
        public void ReinitialiserFiltres()
        {
            _filtreZone = null;
            _filtreLibelle = null;
            _gestionBouteille.Bouteilles = new ObservableCollection<Bouteille>(_gestionBouteille.AllBouteilles);
            OnPropertyChanged(nameof(Bouteilles));
        }
    }
}