using System;
using Potestas.Interfaces;
using Potestas.Processors.Save;
using Potestas.Storages;

namespace Potestas.ConcreteFactories
{
    public class SaveToBsonProcessorFactory : IProcessingFactory
    {
        private IEnergyObservationStorage<IEnergyObservation> _storage;

        public IEnergyObservationAnalizer<IEnergyObservation> CreateAnalizer()
        {
            return null;
        }

        public IEnergyObservationProcessor<IEnergyObservation> CreateProcessor()
        {
            return new SaveToBsonProcessor<IEnergyObservation>();
        }

        public IEnergyObservationStorage<IEnergyObservation> CreateStorage()
        {
            return _storage ?? (_storage = new BsonStorage<IEnergyObservation>());
        }
    }
}
