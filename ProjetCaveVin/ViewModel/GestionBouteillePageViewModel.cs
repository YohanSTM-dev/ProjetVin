using ProjetCaveVin.Model.Classes;
using ProjetCaveVin.Model.Services;
using ProjetCaveVin.Helpers;
using System.Collections.ObjectModel;
using System.Linq;

namespace ProjetCaveVin.ViewModel
{
    public class GestionBouteillePageViewModel : BaseViewModel
    {
        private readonly IBouteilleService _bouteilleService;
        private readonly IZoneService _zoneService;

        private ObservableCollection<Bouteille> _bouteilles;
        private ObservableCollection<Zone> _zones;
        private string _filtreZone;
        private string _filtreLibelle;

        // Cache pour toutes les bouteilles
        private List<Bouteille> _allBouteilles;

        public ObservableCollection<Bouteille> Bouteilles
        {
            get => _bouteilles;
            set
            {
                _bouteilles = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Zone> Zones
        {
            get => _zones;
            set
            {
                _zones = value;
                OnPropertyChanged();
            }
        }

        public GestionBouteillePageViewModel()
        {
            // Injection de dépendances (peut être amélioré avec un conteneur IoC)
            _bouteilleService = new BouteilleService();
            _zoneService = new ZoneService();

            ChargerDonnees();
        }

        private void ChargerDonnees()
        {
            // Charger les zones
            Zones = new ObservableCollection<Zone>(_zoneService.GetAllZones());

            // Charger toutes les bouteilles
            _allBouteilles = _bouteilleService.GetAllBouteilles();
            Bouteilles = new ObservableCollection<Bouteille>(_allBouteilles);
        }

        public void RechargerBouteilles()
        {
            _filtreZone = null;
            _filtreLibelle = null;
            ChargerDonnees();
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
                ? _allBouteilles
                : _bouteilleService.GetBouteillesParZone(_filtreZone);

            // Appliquer le filtre sur le libellé si présent
            if (!string.IsNullOrWhiteSpace(_filtreLibelle))
            {
                bouteillesFiltrees = bouteillesFiltrees
                    .Where(b => b.Libelle.ToLower().Contains(_filtreLibelle.ToLower()))
                    .ToList();
            }

            Bouteilles = new ObservableCollection<Bouteille>(bouteillesFiltrees);
        }

        public void ReinitialiserFiltres()
        {
            _filtreZone = null;
            _filtreLibelle = null;
            Bouteilles = new ObservableCollection<Bouteille>(_allBouteilles);
        }
    }
}