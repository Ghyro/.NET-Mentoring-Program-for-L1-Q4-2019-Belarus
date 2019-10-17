using Potestas.Interfaces;
using Potestas.Processors.Save;
using System;

namespace Potestas.ConcreteFactories
{
    public class SaveToStorageProcessorFactory<T> : IProcessingFactory<T> where T : IEnergyObservation
    {
        public IEnergyObservationProcessor<T> CreateProcessor(IStorageFactory<T> storageFactory = null, IProcessingFactory<T> processorFactory = null)
        {
            if (storageFactory == null)            
                throw new ArgumentNullException();            

            return new SaveToStorageProcessor<T>(storageFactory.CreateStorage());
        }
    }
}
