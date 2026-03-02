using ProjetCaveVin.Helpers;
using ProjetCaveVin.Model.Classes;
using ProjetCaveVin.Repositories;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ProjetCaveVin.ViewModel
{
    public class StatistiquesGrandMonarqueViewModel : BaseViewModel
    {

        private ObservableCollection<Bouteille> _listeBouteilles;
        public ObservableCollection<Bouteille> ListeBouteilles
        {
            get => _listeBouteilles;
            set { _listeBouteilles = value; OnPropertyChanged(); }
        }

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
        private BouteilleRepository BouteilleRepository { get; set; } = new BouteilleRepository();
        private Bouteille _selectedBouteille;
        public Bouteille SelectedBouteille
        {
            get => _selectedBouteille;
            set
            {
                _selectedBouteille = value;
                OnPropertyChanged();
                //on force le bouton à revérifier s'il peut être cliqué
                //(DeplacerCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        // L'ID de l'emplacement où on veut l'envoyer (saisi dans un TextBox)
        private int _targetEmplacementId;
        public int TargetEmplacementId
        {
            get => _targetEmplacementId;
            set { _targetEmplacementId = value; OnPropertyChanged(); }
        }

        public ICommand DeplacerCommand { get; }
        public ICommand RefreshCommand { get; }

        public StatistiquesGrandMonarqueViewModel()
        {
            if (Session.CurrentUser != null)
                BonjourUser = $"Bonjour, {Session.CurrentUser.Prenom}";
            else
                BonjourUser = "Mode Invité";

            DeplacerCommand = new RelayCommand(ExecuterDeplacement, PeutDeplacer);
            RefreshCommand = new RelayCommand(ChargerDonnees);

            ChargerDonnees();
        }


        public void ChargerDonnees()
        {
            var liste = BouteilleRepository.GetAllBouteillesAvecEmplacement();

            ListeBouteilles = new ObservableCollection<Bouteille>(liste);
            CoutTotal = liste.Sum(b => b.Prix);
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

            HDRepository.EnregistrerMouvement(
                SelectedBouteille.Id,
                TargetEmplacementId,
                Session.CurrentUser.id_utilisateur
            );

            MessageBox.Show($"Bouteille déplacée vers l'emplacement ID {TargetEmplacementId} !");

            ChargerDonnees();
            TargetEmplacementId = 0; // Reset du champ
        }
    }
}