using Potestas.Analizers;
using Potestas.Context;
using Potestas.Interfaces;
using Potestas.Processors.Save;
using Potestas.Storages;

namespace Potestas.ConcreteFactories
{
    public class SaveToSqlWithOrmProcessorFactory : IProcessingFactory
    {
        private IEnergyObservationStorage<IEnergyObservation> _storage;

        public IEnergyObservationAnalizer<IEnergyObservation> CreateAnalizer()
        {
            return new SqlOrmAnalyzer<IEnergyObservation>(new ObservationContext());
        }

        public IEnergyObservationProcessor<IEnergyObservation> CreateProcessor()
        {
            return new SaveToSqlWithOrmProcessor<IEnergyObservation>(new ObservationContext());
        }

        public IEnergyObservationStorage<IEnergyObservation> CreateStorage()
        {
            return _storage ?? (_storage = new SqlOrmStorage<IEnergyObservation>(new ObservationContext()));
        }
    }
}
