using System.Collections.Generic;

namespace Potestas.Storages
{
    public class ListStorage : List<IEnergyObservation>, IEnergyObservationStorage
    {
        public string Description => "Simple in-memory storage of energy observations";
    }
}
