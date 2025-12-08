using ProjetCaveVin.Model.Classes;
using System.Collections.Generic;

namespace ProjetCaveVin.Model.Services
{
    public interface IBouteilleService
    {
        List<Bouteille> GetAllBouteilles();
        List<Bouteille> GetBouteillesParZone(string codeZone);
    }
}