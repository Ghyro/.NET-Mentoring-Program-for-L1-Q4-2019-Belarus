using Potestas.Interfaces;
using Potestas.Analizers;

namespace Potestas.ConcreteFactories
{
    public class LINQAnalizerFactory<T> : IAnalizerFactory<T> where T: IEnergyObservation
    {
        public IEnergyObservationAnalizer<T> CreateAnalizer<T>(IStorageFactory<T> storageFactory = null) where T : IEnergyObservation
        {
            return new LINQAnalizer<T>(storageFactory.CreateStorage());
        }
    }
}
