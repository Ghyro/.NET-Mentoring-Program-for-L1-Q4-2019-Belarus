using Potestas.Storages;
using Potestas.Interfaces;

namespace Potestas.ConcreteFactories
{
    public class FileStorageFactory<T> : IStorageFactory<T> where T : IEnergyObservation
    {
        public IEnergyObservationStorage<T> CreateStorage()
        {
            return new FileStorage<T>(@"C:\test_potestas.json");
        }
    }
}
