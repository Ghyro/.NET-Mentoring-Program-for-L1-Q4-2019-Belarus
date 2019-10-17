using System;
using System.Threading;
using System.Threading.Tasks;

namespace Potestas.Interfaces
{
    /* TASK. Refactor these interfaces to avoid boxing and unboxing specific issues
     * QUESTIONS:
     * 1. What is the purpose of Observable pattern?
     * 2. Which types of constraints can be used in generics?
     * 3. Compare IEnergyObservationSource and IEnergyObservationEventSource. 
     * Why IEnergyObservationSource is enough to implement Observable? Which option do you prefere? 
     */
    public interface IEnergyObservationSource<T> : IObservable<T> where T : IEnergyObservation
    {
        string Description { get; }

        Task Run(CancellationToken cancellationToken);
    }

    public interface IEnergyObservationEventSource<T> : IEnergyObservationSource<T> where T: IEnergyObservation
    {
        event EventHandler<T> NewValueObserved;

        event EventHandler<Exception> ObservationError;

        event EventHandler ObservationEnd;
    }
}
