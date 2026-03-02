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

        // Repositories
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

        // Bouteille sélectionnée dans le tableau
        private Bouteille _selectedBouteille;
        public Bouteille SelectedBouteille
        {
            get => _selectedBouteille;
            set
            {
                _selectedBouteille = value;
                OnPropertyChanged();
                // Met à jour l'état du bouton "Déplacer"
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

        public StatistiquesGrandMonarqueViewModel()
        {
            if (Session.CurrentUser != null)
                BonjourUser = $"Bonjour, {Session.CurrentUser.Prenom}";
            else
                BonjourUser = "Mode Invité";

            _toutesLesBouteilles = new List<Bouteille>();
            ListeBouteilles = new ObservableCollection<Bouteille>();

            DeplacerCommand = new RelayCommand(ExecuterDeplacement, PeutDeplacer);
            RefreshCommand = new RelayCommand(ChargerDonnees);

            ChargerDonnees();
        }

        public void ChargerDonnees()
        {
            // Utilisation du Repository entrant combiné avec ta variable
            var listeBrute = BouteilleRepository.GetAllBouteillesAvecEmplacement();

            _toutesLesBouteilles = listeBrute ?? new List<Bouteille>();

            // On convertit la List en ObservableCollection
            ListeBouteilles = new ObservableCollection<Bouteille>(_toutesLesBouteilles);

            CalculerTotal();

            // calculer les stats par emplacement pour l'autre onglet 
            if (_toutesLesBouteilles != null)
            {
                var stats = _toutesLesBouteilles.Where(b => !string.IsNullOrEmpty(b.Code_Emplacement)).GroupBy(b => b.Code_Emplacement).Select(g => new StatistiqueZone
                {
                    NomZone = g.Key,
                    Quantite = g.Count(),
                    ValeurTotale = g.Sum(b => b.Prix)
                }).OrderBy(s => s.NomZone).ToList();

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

        private void ExecuterDeplacement()
        {
            if (SelectedBouteille == null) return;

            if (TargetEmplacementId <= 0)
            {
                MessageBox.Show("Veuillez saisir un ID d'emplacement valide (ex: 1, 2...).");
                return;
            }

            try
            {
                // Utilisation du HDRepository avec ta gestion d'erreur (try/catch)
                HDRepository.EnregistrerMouvement(
                    SelectedBouteille.Id,
                    TargetEmplacementId,
                    Session.CurrentUser.id_utilisateur
                );

                MessageBox.Show($"Succès ! Bouteille déplacée vers l'emplacement ID {TargetEmplacementId}.");

                TargetEmplacementId = 0;

                ChargerDonnees();

                TexteRecherche = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du déplacement : {ex.Message}");
            }
        }
    }

    public class StatistiqueZone
    {
        public string NomZone { get; set; } // Ex: "A1" ou "B"
        public int Quantite { get; set; }
        public decimal ValeurTotale { get; set; }
        public int Limite { get; set; }

        public string Remplisage => $"{Quantite} / {Limite}"; // Ex: "5 / 20"

        public bool EstPlein => Quantite >= Limite;
    }
}