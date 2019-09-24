namespace Potestas
{
    /* TASK. Refactor these interfaces to create families of IObserver, IObservable and IObservationsRepository as a single responsibility. 
     * QUESTIONS:
     * Which pattern is used here?
     * Why factory interface is needed here?
     */
    public interface ISourceFactory
    {
        IEnergyObservationSource CreateSource();

        IEnergyObservationEventSource CreateEventSource();
    }

    public interface IProcessingFactory
    {
        IEnergyObservationProcessor CreateProcessor();

        IEnergyObservationStorage CreateStorage();

        IEnergyObservationAnalizer CreateAnalizer();
    }
}
