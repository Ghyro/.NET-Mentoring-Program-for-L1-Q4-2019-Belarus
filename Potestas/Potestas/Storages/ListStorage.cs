using System.Collections.Generic;
using System.Linq;
using Potestas.Interfaces;

namespace Potestas.Storages
{
    public class ListStorage<T> : List<T>, IEnergyObservationStorage<T> where T: IEnergyObservation
    {
        public string Description => "Simple in-memory storage of energy observations";

        public IEnumerable<T> GetAll()
        {
            return this;
        }

        public T GetByHash(int hashCode)
        {
            return this.Single(item => item.GetHashCode() == hashCode);
        }
    }
}
