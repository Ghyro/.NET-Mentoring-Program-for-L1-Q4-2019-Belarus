using System.Configuration;
using System.IO;
using Potestas.Analizers;
using Potestas.Interfaces;
using Potestas.Processors.Save;
using Potestas.Processors.Serializers;
using Potestas.Storages;

namespace Potestas.ConcreteFactories
{
    public class SaveToFileProcessorFactory : IProcessingFactory
    {
        private IEnergyObservationStorage<IEnergyObservation> _storage = null;

        public IEnergyObservationAnalizer<IEnergyObservation> CreateAnalizer()
        {
            return new LINQAnalizer<IEnergyObservation>(CreateStorage());
        }

        public IEnergyObservationProcessor<IEnergyObservation> CreateProcessor()
        {
            return new SaveToFileProcessor<IEnergyObservation>(new JsonSerializeProcessor<IEnergyObservation>
            {
                Stream = new FileStream(ConfigurationManager.AppSettings["storagePath"], FileMode.OpenOrCreate)
            }, ConfigurationManager.AppSettings["processorPath"]);
        }

        public IEnergyObservationStorage<IEnergyObservation> CreateStorage()
        {
            if (_storage == null)
                _storage = new FileStorage<IEnergyObservation>(ConfigurationManager.AppSettings["storagePath"]);
            return _storage;
        }
    }
}
