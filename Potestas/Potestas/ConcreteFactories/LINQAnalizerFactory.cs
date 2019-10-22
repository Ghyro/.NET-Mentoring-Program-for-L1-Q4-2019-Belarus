using Potestas.Analizers;
using Potestas.Interfaces;

namespace Potestas.ConcreteFactories
{
    public class LINQAnalizerFactory<T> : IAnalizerFactory<T> where T : IEnergyObservation
    {
        public IEnergyObservationAnalizer<T> CreateAnalizer(IEnergyObservationStorage<IEnergyObservation> observationStorage)
        {
            return new LINQAnalizer<T>(observationStorage);
        }
    }
}
