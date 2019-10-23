using System.Configuration;
using Potestas.Analizers;
using Potestas.Interfaces;
using Potestas.Processors.Serializers;
using Potestas.Storages;

namespace Potestas.ConcreteFactories
{
    public class SerializeProcessingFactory : IProcessingFactory
    {
        private IEnergyObservationStorage<IEnergyObservation> _storage = null;

        public IEnergyObservationAnalizer<IEnergyObservation> CreateAnalizer()
        {
            return new LINQAnalizer<IEnergyObservation>(CreateStorage());
        }

        public IEnergyObservationProcessor<IEnergyObservation> CreateProcessor()
        {
            return new SerializeProcessor<IEnergyObservation>();
        }

        public IEnergyObservationStorage<IEnergyObservation> CreateStorage()
        {
            if (_storage == null)
                _storage = new FileStorage<IEnergyObservation>(ConfigurationManager.AppSettings.Get("storagePath"));
            return _storage;
        }
    }
}
