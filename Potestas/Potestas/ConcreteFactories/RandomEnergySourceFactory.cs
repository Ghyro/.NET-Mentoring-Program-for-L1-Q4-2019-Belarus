using Potestas.Interfaces;
using Potestas.Sources;
using System;

namespace Potestas.ConcreteFactories
{
    public class RandomEnergySourceFactory<T> : ISourceFactory<T> where T : IEnergyObservation
    {
        public IEnergyObservationEventSource<T> CreateEventSource()
        {
            throw new NotImplementedException();
        }

        public IEnergyObservationSource CreateSource()
        {
            return new RandomEnergySource();
        }
    }
}
