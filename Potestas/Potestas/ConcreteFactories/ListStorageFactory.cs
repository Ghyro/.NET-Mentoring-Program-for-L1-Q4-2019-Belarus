using Potestas.Interfaces;
using Potestas.Storages;

namespace Potestas.ConcreteFactories
{
    public class ListStorageFactory<T> : IStorageFactory<T> where T : IEnergyObservation
    {
        public IEnergyObservationStorage<T> CreateStorage()
        {
            return new ListStorage<T>();
        }
    }
}
