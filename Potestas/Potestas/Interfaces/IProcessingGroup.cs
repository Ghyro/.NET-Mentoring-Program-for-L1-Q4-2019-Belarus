﻿namespace Potestas.Interfaces

{
    public interface IProcessingGroup<T> where T: IEnergyObservation
    {
        IEnergyObservationProcessor<T> Processor { get; }

        IEnergyObservationStorage<T> Storage { get; }

        IEnergyObservationAnalyzer<T> Analizer { get; }

        void Detach();
    }
}
