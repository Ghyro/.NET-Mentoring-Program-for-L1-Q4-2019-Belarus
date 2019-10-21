using Potestas.Interfaces;
using Potestas.Processors.Serializers;
using Potestas.Storages;

namespace Potestas.ConcreteFactories
{
    public class StorageFactory<T> : IStorageFactory<T> where T : IEnergyObservation
    {
        public IEnergyObservationStorage<IEnergyObservation> CreateFileStorage(string filePath, SerializeProcessor<IEnergyObservation> serializer)
        {
            return new FileStorage<IEnergyObservation>(filePath);
        }

        public IEnergyObservationStorage<IEnergyObservation> CreateListStorage()
        {
            return new ListStorage<IEnergyObservation>();
        }
    }
}
