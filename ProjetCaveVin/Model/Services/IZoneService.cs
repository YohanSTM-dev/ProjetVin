using ProjetCaveVin.Model.Classes;
using System.Collections.Generic;

namespace ProjetCaveVin.Model.Services
{
    public interface IZoneService
    {
        List<Zone> GetAllZones();
    }
}