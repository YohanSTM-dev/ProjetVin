using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetCaveVin.Helpers;
using ProjetCaveVin.Model.Classes;

namespace ProjetCaveVin.ViewModel
{ 
public class statsZoneViewModel : BaseViewModel
    {


        public string NomZone { get; set; }
        public int Quantite { get; set; }
        public decimal ValeurTotal { get; set; }
    }

}