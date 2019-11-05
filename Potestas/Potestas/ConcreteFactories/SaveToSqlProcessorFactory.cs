using Potestas.Analizers;
using Potestas.Interfaces;
using Potestas.Processors.Save;
using Potestas.Storages;
using System.Configuration;

namespace Potestas.ConcreteFactories
{
    public class SaveToSqlProcessorFactory : IProcessingFactory
    {
        private IEnergyObservationStorage<IEnergyObservation> _storage;

        public IEnergyObservationAnalizer<IEnergyObservation> CreateAnalizer()
        {
            return new SqlAnalyzer<IEnergyObservation>(ConfigurationManager.AppSettings["ADOConnection"]);
        }

        public IEnergyObservationProcessor<IEnergyObservation> CreateProcessor()
        {
            return new SaveToSqlProcessor<IEnergyObservation>();
        }

        public IEnergyObservationStorage<IEnergyObservation> CreateStorage()
        {
            return _storage ?? (_storage = new SqlStorage<IEnergyObservation>());
        }
    }
}
