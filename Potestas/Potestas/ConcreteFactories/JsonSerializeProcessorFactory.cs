using Potestas.Interfaces;
using Potestas.Processors.Serializers;

namespace Potestas.ConcreteFactories
{
    public class JsonSerializeProcessorFactory<T> : IProcessingFactory<T> where T : IEnergyObservation
    {
        public IEnergyObservationProcessor<T> CreateProcessor(IStorageFactory<T> storageFactory = null, IProcessingFactory<T> processorFactory = null)
        {
            return new JsonSerializeProcessor<T>();
        }
    }
}
