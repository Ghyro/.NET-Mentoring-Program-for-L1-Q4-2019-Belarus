using Potestas.Analizers;
using Potestas.Interfaces;
using Potestas.Processors.Save;
using Potestas.Storages;

namespace Potestas.ConcreteFactories
{
    public class SaveToStorageProcessorFactory : IProcessingFactory
    {
        private IEnergyObservationStorage<IEnergyObservation> _storage;

        public IEnergyObservationAnalizer<IEnergyObservation> CreateAnalizer()
        {
            return new LINQAnalyzer<IEnergyObservation>(CreateStorage());
        }

        public IEnergyObservationProcessor<IEnergyObservation> CreateProcessor()
        {
            return new SaveToStorageProcessor<IEnergyObservation>(CreateStorage());
        }

        public IEnergyObservationStorage<IEnergyObservation> CreateStorage()
        {
            return _storage ?? (_storage = new FileStorage<IEnergyObservation>());
        }
    }
}
