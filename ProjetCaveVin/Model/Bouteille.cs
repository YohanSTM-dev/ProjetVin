using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCaveVin.Model
{
    internal class Bouteille
    {

        public int Id { get; set; }
        public string Nom { get; set; }
        
        public int Millesime { get; set; }
        public int Quantite { get; set; }

    }
}
