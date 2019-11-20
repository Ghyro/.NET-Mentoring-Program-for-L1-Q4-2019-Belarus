using Potestas.Analyzers;
using Potestas.Interfaces;
using Potestas.Processors.Serializers;
using Potestas.Storages;

namespace Potestas.ConcreteFactories
{
    public class SerializeToXMLProcessorFactory : IProcessingFactory
    {
        private IEnergyObservationStorage<IEnergyObservation> _storage;

        public IEnergyObservationAnalizer<IEnergyObservation> CreateAnalizer()
        {
            return new XMLAnalyzer<IEnergyObservation>();
        }

        public IEnergyObservationProcessor<IEnergyObservation> CreateProcessor()
        {
            return new SerializeToXMLProcessor<IEnergyObservation>();
        }

        public IEnergyObservationStorage<IEnergyObservation> CreateStorage()
        {
            return _storage ?? (_storage = new XmlFileStorage<IEnergyObservation>());
        }
    }
}
