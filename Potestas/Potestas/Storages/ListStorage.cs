using System.Collections.Generic;
using System.Linq;
using Potestas.Interfaces;

namespace Potestas.Storages
{
    public class ListStorage : List<IEnergyObservation>, IEnergyObservationStorage<IEnergyObservation>
    {
        public string Description => "Simple in-memory storage of energy observations";

        public IEnumerable<IEnergyObservation> GetAll()
        {
            return this;
        }

        public IEnergyObservation GetByHash(int hashCode)
        {
            return this.SingleOrDefault(item => item.GetHashCode() == hashCode);
        }
    }
}
