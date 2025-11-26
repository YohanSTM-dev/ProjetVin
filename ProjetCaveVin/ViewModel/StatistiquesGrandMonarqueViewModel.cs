using ProjetCaveVin.Helpers;
using ProjetCaveVin.Model.Classes;
using ProjetCaveVin.Model.Connexion;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;


namespace ProjetCaveVin.ViewModel
{
    public class StatistiquesGrandMonarqueViewModel : BaseViewModel
    {
        private decimal _coutTotal;
        public decimal CoutTotal
        {
            get { return _coutTotal; }
            set { _coutTotal = value; OnPropertyChanged(); }
        }


        private Dictionary<string, int> _stocksParBouteille;
        public Dictionary<string, int> StocksParBouteille
        {
            get { return _stocksParBouteille; }
            set { _stocksParBouteille = value; OnPropertyChanged(); }
        }



        private Dictionary<string, int> _repartitionParType;
        public Dictionary<string, int> RepartitionParType
        {
            get { return _repartitionParType; }
            set { _repartitionParType = value; OnPropertyChanged(); }
        }

        private List<HistoriqueDeplacement> _historiqueList;
        public List<HistoriqueDeplacement> HistoriqueList
        {
            get { return _historiqueList; }
            set { _historiqueList = value; OnPropertyChanged(); }
        }

        public ICommand TestDeplacementCommand { get; private set; }

        public StatistiquesGrandMonarqueViewModel()
        {
            TestDeplacementCommand = new RelayCommand(SimulerDeplacement);
            ChargerDonnees();
        }

        public void ChargerDonnees()
        {
            List<Bouteille> toutesLesBouteilles = Bouteille.getAllBouteilles();

            CoutTotal = CalculerValeurStock(toutesLesBouteilles);

            this.StocksParBouteille = toutesLesBouteilles
                .GroupBy(b => b.Libelle)
                .ToDictionary(g => g.Key, g => g.Count());

            this.RepartitionParType = toutesLesBouteilles.GroupBy(b => b.Type).ToDictionary(g => g.Key, g => g.Count());

            //afficher Historique
            HistoriqueList = HistoriqueDeplacement.GetAllHistoriqueDeplacements();
        }

        private decimal CalculerValeurStock(List<Bouteille> listeDeBouteilles) // calcule la valeur totale du stock
        {
            if (listeDeBouteilles == null || !listeDeBouteilles.Any()) return 0;
            return listeDeBouteilles.Sum(b => b.Prix);
        }

        public void DeplacerUneBouteille(int idBouteille, int idNouveauEmplacement, int idUtilisateur)
        {
            HistoriqueDeplacement.EnregistrerMouvement(idBouteille, idNouveauEmplacement, idUtilisateur);

            ChargerDonnees();
        }

        private void AfficherHistoriqueDeplacement(HistoriqueDeplacement historique)
        {
            if(historique == null) { return;}

        }

        private void SimulerDeplacement()
        {
            if(Session.CurrentUser == null)
            {
                System.Windows.MessageBox.Show("Erreur: Vous êtes connecter avec aucun compte");
                return;
            }
            int idBouteilleTest = 15;
            int idEmplacementTest = 14;
            int idUtilisateur = Session.CurrentUser.id_utilisateur;

            

            DeplacerUneBouteille(idBouteilleTest, idEmplacementTest, idUtilisateur);
        }


    }
}