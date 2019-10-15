﻿using Potestas.Interfaces;

namespace Potestas
{
    /* TASK. Refactor these interfaces to create families of IObserver, IObservable and IObservationsRepository as a single responsibility. 
     * QUESTIONS:
     * Which pattern is used here?
     * Why factory interface is needed here?
     */
    public interface ISourceFactory
    {
        IEnergyObservationSource<IEnergyObservation> CreateSource();

        IEnergyObservationEventSource CreateEventSource();
    }

    public interface IProcessingFactory
    {
        IEnergyObservationProcessor<IEnergyObservation> CreateProcessor();

        IEnergyObservationStorage<IEnergyObservation> CreateStorage();

        IEnergyObservationAnalizer CreateAnalizer(IEnergyObservationStorage<IEnergyObservation> observationStorage);
    }
}
