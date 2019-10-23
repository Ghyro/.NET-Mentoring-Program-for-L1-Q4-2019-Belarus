using Potestas.Interfaces;
using Potestas.Sources;
using System;

namespace Potestas.ConcreteFactories
{
    public class RandomEnergySourceFactory : ISourceFactory
    {
        public IEnergyObservationEventSource<IEnergyObservation> CreateEventSource()
        {
            throw new NotImplementedException();
        }

        public IEnergyObservationSource CreateSource()
        {
            return new RandomEnergySource();
        }
    }
}
