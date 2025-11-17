using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetCaveVin.Model.Classes;

namespace ProjetCaveVin.Model
{
    namespace TonProjet.Models
    {
        public class Utilisateur
        {
            public int Id { get; set; }
            public string Nom { get; set; }
            public string Prenom { get; set; }
            public string Role { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }

            public List<HistoriqueDeplacement> HistoriqueDeplacements { get; set; } = new List<HistoriqueDeplacement>();
        }
    }

}
