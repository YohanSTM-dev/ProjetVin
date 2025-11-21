using ProjetCaveVin.Helpers;
using ProjetCaveVin.Model.Classes;
using System.Collections.Generic;
using System.Linq;

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

        public StatistiquesGrandMonarqueViewModel()
        {
            ChargerDonnees();
        }

        public void ChargerDonnees()
        {
            List<Bouteille> bouteilles = Bouteille.getAllBouteilles();

            // Calcul du prix total
            CoutTotal = CalculerValeurStock(bouteilles);

            StocksParBouteille = bouteilles .GroupBy(b => b.Libelle).ToDictionary(g => g.Key, g => g.Count());
        }

        private decimal CalculerValeurStock(List<Bouteille> listeDeBouteilles)
        {
            if (listeDeBouteilles == null || !listeDeBouteilles.Any()) return 0;
            return listeDeBouteilles.Sum(b => b.Prix);
        }
    }
}