using System.Configuration;
using Potestas.Analizers;
using Potestas.Interfaces;
using Potestas.Processors.Serializers;
using Potestas.Storages;

namespace Potestas.ConcreteFactories
{
    public class JsonSerializeProcessorFactory : IProcessingFactory
    {
        private IEnergyObservationStorage<IEnergyObservation> _storage = null;

        public IEnergyObservationProcessor<IEnergyObservation> CreateProcessor()
        {
            return new JsonSerializeProcessor<IEnergyObservation>();
        }

        public IEnergyObservationAnalizer<IEnergyObservation> CreateAnalizer()
        {
            return new LINQAnalizer<IEnergyObservation>(CreateStorage());
        }

        public IEnergyObservationStorage<IEnergyObservation> CreateStorage()
        {
            if (_storage == null)
                _storage = new FileStorage<IEnergyObservation>(ConfigurationManager.AppSettings.Get("storagePath"));
            return _storage;
        }
    }
}
