using Potestas.Interfaces;
using Potestas.Processors.Serializers;
using System.IO;

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

        IEnergyObservationEventSource<IEnergyObservation> CreateEventSource();
    }

    public interface IProcessingFactory
    {
        IEnergyObservationProcessor<IEnergyObservation> CreateProcessor();

        IEnergyObservationAnalyzer<IEnergyObservation> CreateAnalizer();

        IEnergyObservationStorage<IEnergyObservation> CreateStorage();
    }
}
