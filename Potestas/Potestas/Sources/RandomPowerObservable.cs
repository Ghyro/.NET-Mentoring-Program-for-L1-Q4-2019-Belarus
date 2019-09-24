using System;
using System.Threading;
using System.Threading.Tasks;

namespace Potestas.Sources
{
    /* TASK. Implement random observation source.
     * 1. This class should generate observations by random periods of time.
     * 1. Implement both IEnergyObservationSource and IEnergyObservationSourceEventSource interfaces.
     * 2. Try to implement it with abstract class or delegate parameters to make it universal.
     */
    public class RandomEnergySource : IEnergyObservationSource
    {
        public string Description => throw new NotImplementedException();

        public Task Run(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe(IObserver<IEnergyObservation> observer)
        {
            throw new NotImplementedException();
        }
    }
}
