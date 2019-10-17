﻿using Potestas.Interfaces;

namespace Potestas
{
    /* TASK. Refactor these interfaces to create families of IObserver, IObservable and IObservationsRepository as a single responsibility. 
     * QUESTIONS:
     * Which pattern is used here?
     * Why factory interface is needed here?
     */
    public interface ISourceFactory<T> where T : IEnergyObservation
    {
        IEnergyObservationSource<T> CreateSource();

        IEnergyObservationEventSource<T> CreateEventSource();
    }

    public interface IProcessingFactory<T> where T : IEnergyObservation
    {
        IEnergyObservationProcessor<T> CreateProcessor(IStorageFactory<T> storageFactory = null, IProcessingFactory<T> processorFactory = null);
    }

    public interface IStorageFactory<T> where T : IEnergyObservation
    {
        IEnergyObservationStorage<T> CreateStorage();
    }

    public interface IAnalizerFactory<T> where T: IEnergyObservation
    {
        IEnergyObservationAnalizer<T> CreateAnalizer<T>(IStorageFactory<T> storageFactory = null) where T : IEnergyObservation;
    }
}
