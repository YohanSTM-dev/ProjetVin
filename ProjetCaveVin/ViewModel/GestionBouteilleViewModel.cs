using System.Linq;
using System.Windows.Input;
using ProjetCaveVin.Helpers;
using ProjetCaveVin.View;
using Microsoft.Win32;
using System;
using System.Collections.Generic;

namespace ProjetCaveVin.ViewModel
{
    public class GestionBouteilleViewModel : BaseViewModel
    {
        // Propriétés pour les statistiques
        private int _totalVentes;
        public int TotalVentes
        {
            get => _totalVentes;
            set { _totalVentes = value; OnPropertyChanged(nameof(TotalVentes)); }
        }
        private decimal _revenuTotal;
        public decimal RevenuTotal
        {
            get => _revenuTotal;
            set { _revenuTotal = value; OnPropertyChanged(nameof(RevenuTotal)); }
        }
        public GestionBouteilleViewModel()
        {
        }
    }
}