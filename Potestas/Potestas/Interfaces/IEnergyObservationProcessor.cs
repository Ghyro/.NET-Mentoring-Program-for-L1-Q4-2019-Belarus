using System;

namespace Potestas.Interfaces
{
    /* TASK. Refactor this interface to avoid boxing and unboxing specific issues. Use generics and contrvariant approach.
     * QUESTIONS:
     * What is the purpose of Observable pattern?
     */
    public interface IEnergyObservationProcessor<in T> : IObserver<T> where T : IEnergyObservation
    {
        string Description { get; }
    }
}
