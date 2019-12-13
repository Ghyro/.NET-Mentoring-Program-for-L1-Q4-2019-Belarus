using Potestas.Analyzers;
using Potestas.Interfaces;
using Potestas.Processors.Serializers;
using Potestas.Storages;

namespace Potestas.ConcreteFactories
{
    public class JsonSerializeProcessorFactory : IProcessingFactory
    {
        private IEnergyObservationStorage<IEnergyObservation> _storage;

        public IEnergyObservationProcessor<IEnergyObservation> CreateProcessor()
        {
            return new JsonSerializeProcessor<IEnergyObservation>();
        }

        public IEnergyObservationAnalyzer<IEnergyObservation> CreateAnalizer()
        {
            return new LINQAnalyzer<IEnergyObservation>(CreateStorage());
        }

        public IEnergyObservationStorage<IEnergyObservation> CreateStorage()
        {
            return _storage ?? (_storage = new FileStorage<IEnergyObservation>());
        }
    }
}
