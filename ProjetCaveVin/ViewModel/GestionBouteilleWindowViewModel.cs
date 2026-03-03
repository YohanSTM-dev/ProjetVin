using ProjetCaveVin.Model.Classes;
using ProjetCaveVin.Model.Services;
using ProjetCaveVin.Helpers;
using System.Collections.ObjectModel;
using System.Linq;
using ProjetCaveVin.Repositories;

namespace ProjetCaveVin.ViewModel
{
    public class GestionBouteilleWindowViewModel : BaseViewModel
    {
        private readonly BouteilleRepository _bouteilleRepo;
        private readonly ZoneRepository _zoneRepo;

        private ObservableCollection<Bouteille> _bouteilles;
        private ObservableCollection<Zone> _zones;
        private string _filtreZone;
        private string _filtreLibelle;
        private string _nbBouteille;
        private int _nombreBouteille = 0;
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
        public int NombreBouteille
        {
            get => _nombreBouteille;
            set { _nombreBouteille =  value; OnPropertyChanged(); }
        }
        public string NbBouteille
        {
            get => _nbBouteille;
            set { _nbBouteille = value; OnPropertyChanged(); }
        }


        public GestionBouteilleWindowViewModel()
        {
            _bouteilleRepo = new BouteilleRepository();
            _zoneRepo = new ZoneRepository();

            ChargerDonnees();
        }

        private void ChargerDonnees()
        {
            // Charger les zones
            Zones = new ObservableCollection<Zone>(_zoneRepo.GetAllZones());

            // Charger toutes les bouteilles
            _allBouteilles = _bouteilleRepo.GetAllBouteilles();
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
            var bouteillesFiltrees = string.IsNullOrWhiteSpace(_filtreZone)
                ? _allBouteilles
                : _bouteilleRepo.GetBouteillesParZone(_filtreZone);

            if (!string.IsNullOrWhiteSpace(_filtreLibelle))
            {
                bouteillesFiltrees = bouteillesFiltrees
                    .Where(b => b.Libelle.ToLower().Contains(_filtreLibelle.ToLower()))
                    .ToList();
            }

            Bouteilles = new ObservableCollection<Bouteille>(bouteillesFiltrees);
            NombreBouteille = bouteillesFiltrees.Count;
            NbBouteille = NombreBouteille.ToString() + " Bouteilles";
        }

        public void ReinitialiserFiltres()
        {
            _filtreZone = null;
            _filtreLibelle = null;
            Bouteilles = new ObservableCollection<Bouteille>(_allBouteilles);
            NombreBouteille = Bouteilles.Count;
            NbBouteille = NombreBouteille.ToString() + " Bouteilles";
        }
    }
}