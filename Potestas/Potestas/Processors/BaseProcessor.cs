using System;
using Potestas.Interfaces;

namespace Potestas.Processors
{
    public abstract class BaseProcessor<T> : IEnergyObservationProcessor<T> where T : IEnergyObservation
    {
        public abstract void OnCompleted();

        public abstract void OnError(Exception error);

        public abstract void OnNext(T value);

        public abstract string Description { get; }
    }
}
