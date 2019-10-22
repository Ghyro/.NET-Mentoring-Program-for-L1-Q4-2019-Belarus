using System.Configuration;
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
            return new SaveToFileProcessor<IEnergyObservation>(new JsonSerializeProcessor<IEnergyObservation>())
            {
                FileName = ConfigurationManager.AppSettings.Get("processorPath")
            };
        }

        public IEnergyObservationStorage<IEnergyObservation> CreateStorage()
        {
            if (_storage == null)
                _storage = new FileStorage<IEnergyObservation>(ConfigurationManager.AppSettings.Get("storagePath"));
            return _storage;
        }
    }
}
