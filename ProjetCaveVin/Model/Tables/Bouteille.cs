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
        

        public string Code { get; set; }
        public string Code_Emplacement { get; set; }
        public string Type { get; set; }
        public string Photo { get; set; }

        public int IdOrigine { get; set; }
        //public Origine Origine { get; set; }

        //public List<HistoriqueDeplacement> HistoriqueDeplacements { get; set; } = new List<HistoriqueDeplacement>();

        

        public Bouteille(int id, string libelle, string millesime, decimal contenance, decimal prix, string code, string code_emplacement, string type, string photo, int idOrigine)
        {
            Id = id;
            Libelle = libelle;
            Millesime = millesime;
            Contenance = contenance;
            Prix = prix;
            Code = code;
            Code_Emplacement = code_emplacement;
            Type = type;
            Photo = photo;
            IdOrigine = idOrigine;
        }
    }


}