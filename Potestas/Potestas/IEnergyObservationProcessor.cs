using System;

namespace Potestas
{
    /* TASK. Refactor this interface to avoid boxing and unboxing specific issues. Use generics and contrvariant approach.
     * QUESTIONS:
     * What is the purpose of Observable pattern?
     */
    public interface IEnergyObservationProcessor : IObserver<IEnergyObservation>
    {
        string Description { get; }
    }
}
