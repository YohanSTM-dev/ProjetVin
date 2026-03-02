using ProjetCaveVin.Model.Connexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCaveVin.Model.Classes
{
    public class Bouteille
    {
        public int Id { get; set; }
        public string Libelle { get; set; }
        public string Millesime { get; set; }
        public decimal Contenance { get; set; }
        public decimal Prix { get; set; }
        //------------------------------------------
        public string Code { get; set; }
        public string Code_Emplacement { get; set; }
        public string Type { get; set; }
        public string Photo { get; set; }

        //------------------------------------------
        public int IdOrigine { get; set; }
        public Origine Origine { get; set; }

        //public TypeBouteille Type { get; set; }
      
        public List<HistoriqueDeplacement> HistoriqueDeplacements { get; set; } = new List<HistoriqueDeplacement>();

    }
}
