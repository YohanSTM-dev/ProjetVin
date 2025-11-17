using ProjetCaveVin.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ProjetCaveVin.Helpers;
using ProjetCaveVin.Model.Tables;

namespace ProjetCaveVin.ViewModel
{
    internal class BoutielleViewModel
    {

        public class BouteilleViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<Bouteille> Bouteilles { get; set; }
            public ICommand AjouterBouteilleCommand { get; set; }

            private string _nom;
            public string Nom
            {
                get => _nom;
                set { _nom = value; OnPropertyChanged(nameof(Nom)); }
            }

            private int _millesime;
            public int Millesime
            {
                get => _millesime;
                set { _millesime = value; OnPropertyChanged(nameof(Millesime)); }
            }

            public BouteilleViewModel()
            {
                Bouteilles = new ObservableCollection<Bouteille>();
                AjouterBouteilleCommand = new RelayCommand(AjouterBouteille);
            }

            private void AjouterBouteille()
            {
                Bouteilles.Add(new Bouteille
                {
                    Id = Bouteilles.Count + 1,
                    Nom = this.Nom,
                    Millesime = this.Millesime,
                    Quantite = 1
                });

                Nom = string.Empty;
                Millesime = 0;
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string name) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
