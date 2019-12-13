using System.Configuration;
using System.IO;
using Potestas.Analyzers;
using Potestas.Interfaces;
using Potestas.Processors.Save;
using Potestas.Processors.Serializers;
using Potestas.Storages;

namespace Potestas.ConcreteFactories
{
    public class SaveToFileProcessorFactory : IProcessingFactory
    {
        private IEnergyObservationStorage<IEnergyObservation> _storage;

        public IEnergyObservationAnalyzer<IEnergyObservation> CreateAnalizer()
        {
            return new LINQAnalyzer<IEnergyObservation>(CreateStorage());
        }

        public IEnergyObservationProcessor<IEnergyObservation> CreateProcessor()
        {
            return new SaveToFileProcessor<IEnergyObservation>(new SerializeToXMLProcessor<IEnergyObservation>
            {
                Stream = new FileStream(ConfigurationManager.AppSettings["xmlStoragePath"], FileMode.OpenOrCreate)
            });
        }

        public IEnergyObservationStorage<IEnergyObservation> CreateStorage()
        {
            return _storage ?? (_storage = new XmlFileStorage<IEnergyObservation>());
        }
    }
}
