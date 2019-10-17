using Potestas.Interfaces;
using Potestas.Processors.Save;
using System.Configuration;

namespace Potestas.ConcreteFactories
{
    public class SaveToFileProcessorFactory<T> : IProcessingFactory<T> where T : IEnergyObservation
    {
        public IEnergyObservationProcessor<T> CreateProcessor(IStorageFactory<T> storageFactory = null, IProcessingFactory<T> processorFactory = null)
        {
            if (processorFactory == null)            
                return new SaveToFileProcessor<T>(null)
                {
                    FileName = ConfigurationManager.AppSettings.Get("processorPath")
                };            

            return new SaveToFileProcessor<T>(processorFactory.CreateProcessor())
            {
                FileName = ConfigurationManager.AppSettings.Get("processorPath")
            };
        }
    }
}
