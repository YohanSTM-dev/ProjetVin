using ProjetCaveVin.Helpers;
using ProjetCaveVin.Model.Classes;
using ProjetCaveVin.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;


namespace ProjetCaveVin.ViewModel
{
    public class StatistiquesGrandMonarqueViewModel : BaseViewModel
    {
        private string _nomEmplacement = string.Empty;
        private int _capacite = 0;
        private string _zone = string.Empty;
        private List<Bouteille> _toutesLesBouteilles;

        private ObservableCollection<Bouteille> _listeBouteilles;
        public ObservableCollection<Bouteille> ListeBouteilles
        {
            get => _listeBouteilles;
            set { _listeBouteilles = value; OnPropertyChanged(); }
        }

        // Stats
        private decimal _coutTotal;
        public decimal CoutTotal
        {
            get => _coutTotal;
            set { _coutTotal = value; OnPropertyChanged(); }
        }

        private string _bonjourUser;
        public string BonjourUser
        {
            get => _bonjourUser;
            set { _bonjourUser = value; OnPropertyChanged(); }
        }
        public string NomEmplacement
        {
            get => _nomEmplacement;
            set { _nomEmplacement = value; OnPropertyChanged(); }
        }
        public int Capacite
        {
            get => _capacite;
            set { _capacite = value; OnPropertyChanged(); }
        }
        public string Zone
        {
            get => _zone;
            set { _zone = value; OnPropertyChanged(); }
        }
        // Repositories
        private ZoneRepository _zoneRepository { get; set; } = new();
        private BouteilleRepository BouteilleRepository { get; set; } = new BouteilleRepository();

        // Texte de la barre de recherche
        private string _texteRecherche;
        public string TexteRecherche
        {
            get => _texteRecherche;
            set
            {
                _texteRecherche = value;
                OnPropertyChanged();
                AppliquerFiltre();
            }
        }

        // Bouteille s�lectionn�e dans le tableau
        private Bouteille _selectedBouteille;
        public Bouteille SelectedBouteille
        {
            get => _selectedBouteille;
            set
            {
                _selectedBouteille = value;
                OnPropertyChanged();
                // Met � jour l'�tat du bouton "D�placer"
                (DeplacerCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        // ID du nouvel emplacement (saisi par l'user)
        private int _targetEmplacementId;
        public int TargetEmplacementId
        {
            get => _targetEmplacementId;
            set { _targetEmplacementId = value; OnPropertyChanged(); }
        }

        // Onglet des zones 
        private ObservableCollection<StatistiqueZone> _listeStatsZones;
        public ObservableCollection<StatistiqueZone> ListeStatsZones
        {
            get => _listeStatsZones;
            set { _listeStatsZones = value; OnPropertyChanged(); }
        }

        public ICommand DeplacerCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand CreerEmplacementCommand { get; }

        public StatistiquesGrandMonarqueViewModel()
        {
            if (Session.CurrentUser != null)
                BonjourUser = $"Bonjour {Session.CurrentUser.Prenom}";
            else
                BonjourUser = "Mode Invit� - Vous �tes connect� � aucun compte";

            _toutesLesBouteilles = new List<Bouteille>();
            ListeBouteilles = new ObservableCollection<Bouteille>();

            DeplacerCommand = new RelayCommand(ExecuterDeplacement, PeutDeplacer);
            RefreshCommand = new RelayCommand(ChargerDonnees);
            CreerEmplacementCommand = new RelayCommand(() => CreerEmplacement());
            ChargerDonnees();
        }

        public void ChargerDonnees()
        {
            var listeBrute = BouteilleRepository.GetAllBouteillesAvecEmplacement();

            _toutesLesBouteilles = listeBrute ?? new List<Bouteille>();

            ListeBouteilles = new ObservableCollection<Bouteille>(_toutesLesBouteilles);

            CalculerTotal();

            if (_toutesLesBouteilles != null)
            {
                var stats = _toutesLesBouteilles
    .Where(b => !string.IsNullOrEmpty(b.Code_Emplacement))
    .GroupBy(b => new { b.Zone, b.Code_Emplacement })
    .Select(g => new StatistiqueZone
    {
        Zone = g.Key.Zone,
        NomEmplacement = g.Key.Code_Emplacement,
        Quantite = g.Count(),
        ValeurTotale = g.Sum(b => b.Prix)
    })
    .OrderBy(s => s.Zone)
    .ThenBy(s => s.NomEmplacement)
    .ToList();

                ListeStatsZones = new ObservableCollection<StatistiqueZone>(stats);
            }
        }

        private void AppliquerFiltre()
        {
            // Si la liste principale est vide, on ne fait rien
            if (_toutesLesBouteilles == null) return;



            if (string.IsNullOrWhiteSpace(TexteRecherche))
            {
                ListeBouteilles = new ObservableCollection<Bouteille>(_toutesLesBouteilles);
            }
            else
            {
                // Sinon, on filtre
                var terme = TexteRecherche.ToLower();

                var resultatFiltre = _toutesLesBouteilles.Where(b =>
                    (b.Libelle != null && b.Libelle.ToLower().Contains(terme)) ||
                    (b.Code_Emplacement != null && b.Code_Emplacement.ToLower().Contains(terme))
                ).ToList();

                ListeBouteilles = new ObservableCollection<Bouteille>(resultatFiltre);
            }

            CalculerTotal();
        }

        private void CalculerTotal()
        {
            if (ListeBouteilles != null)
                CoutTotal = ListeBouteilles.Sum(b => b.Prix);
            else
                CoutTotal = 0;
        }

        private bool PeutDeplacer()
        {
            return SelectedBouteille != null;
        }
        public void CreerEmplacement()
        {
            if (Capacite <= 0)
            {
                MessageBox.Show("Veuillez remplir une quantité valide");

            }
            else if (string.IsNullOrWhiteSpace(NomEmplacement))
            {
                MessageBox.Show("Veuillez remplir un nom");

            }
            else if (string.IsNullOrWhiteSpace(Zone))
            {
                MessageBox.Show("Veuillez remplir une zone");

            }
            else
            {
                _zoneRepository.CreerEmplacement(Capacite, NomEmplacement, Zone);
                MessageBox.Show("Emplacement créé !");
            }

        }
        private void ExecuterDeplacement()
        {
            if (SelectedBouteille == null) return;

            if (Session.CurrentUser == null)
            {
                MessageBox.Show("Vous devez etre connecte pour deplacer une bouteille.");
                return;
            }

            if (TargetEmplacementId <= 0)
            {
                MessageBox.Show("Veuillez saisir un ID d'emplacement valide (ex: 1, 2...).");
                return;
            }

            try
            {
                HDRepository.EnregistrerMouvement(
                    SelectedBouteille.Id,
                    TargetEmplacementId,
                    Session.CurrentUser.id_utilisateur
                );

                MessageBox.Show($"Succ�s ! Bouteille d�plac�e vers l'emplacement ID {TargetEmplacementId}.");

                TargetEmplacementId = 0;

                ChargerDonnees();

                TexteRecherche = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du d�placement : {ex.Message}");
            }
        }
    }

    public class StatistiqueZone
    {
        public string ZoneComplete => $"{Zone} - {NomEmplacement}";
        public string Zone { get; set; } 
        public string NomEmplacement { get; set; } // Ex: "A1" ou "B"
        public int Quantite { get; set; }
        public decimal ValeurTotale { get; set; }
        public int Limite { get; set; }

        public string Remplisage => $"{Quantite} / {Limite}"; // Ex: "5 / 20"

        public bool EstPlein => Quantite >= Limite;
    }
}

