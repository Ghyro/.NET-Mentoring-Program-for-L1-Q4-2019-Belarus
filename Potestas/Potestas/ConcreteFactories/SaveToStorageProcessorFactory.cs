using System;
using System.Configuration;
using Potestas.Analizers;
using Potestas.Interfaces;
using Potestas.Processors.Save;
using Potestas.Storages;

namespace Potestas.ConcreteFactories
{
    public class SaveToStorageProcessorFactory : IProcessingFactory
    {
        private IEnergyObservationStorage<IEnergyObservation> _storage = null;

        public IEnergyObservationAnalizer<IEnergyObservation> CreateAnalizer()
        {
            return new LINQAnalizer<IEnergyObservation>(CreateStorage());
        }

        public IEnergyObservationProcessor<IEnergyObservation> CreateProcessor()
        {
            return new SaveToStorageProcessor<IEnergyObservation>(CreateStorage());
        }

        public IEnergyObservationStorage<IEnergyObservation> CreateStorage()
        {
            if (_storage == null)
                _storage = new FileStorage<IEnergyObservation>(ConfigurationManager.AppSettings.Get("processorPath"));
            return _storage;
        }
    }
}
