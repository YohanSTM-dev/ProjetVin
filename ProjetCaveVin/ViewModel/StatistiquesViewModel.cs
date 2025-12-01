using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetCaveVin.Helpers;
using ProjetCaveVin.Model.Classes;

namespace ProjetCaveVin.ViewModel
{
    public class StatistiquesViewModel : BaseViewModel
    {

        public decimal _coutTotal;
        private decimal CoutTotal
        {
            get { return _coutTotal; }
            set { _coutTotal = value; OnPropertyChanged(); }
        }

        public string _nomUtilisateur;
        private string NomUtilisateur
        {
            get { return _nomUtilisateur; }
            set { _nomUtilisateur = value; OnPropertyChanged(); }
        }

        private Dictionary<string, int> _repartitionVins;
        public Dictionary<string, int> RepartitionVins
        {
            get { return _repartitionVins; }
            set { _repartitionVins = value; OnPropertyChanged(); }
        }
        
        public StatistiquesViewModel()
        {
            ChargerDonnees();
        }

        public void ChargerDonnees()
        {
            List<Bouteille> bouteilles = Bouteille.getAllBouteilles();
            var totalCoutBouteilles = bouteilles.Sum(b=>b.Prix); // fais la somme des prix de chaque bouteille
            RepartitionVins = bouteilles.GroupBy(b=>b.Type).ToDictionary(TypeGroup => TypeGroup.Key, TypeGroup => TypeGroup.Count());

        }
    }
}
