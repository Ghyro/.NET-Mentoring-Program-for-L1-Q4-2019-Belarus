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
    public interface ISourceFactory<T> where T : IEnergyObservation
    {
        IEnergyObservationSource CreateSource();

        IEnergyObservationEventSource<T> CreateEventSource();
    }

    public interface IProcessingFactory<T> where T : IEnergyObservation
    {
        IEnergyObservationProcessor<T> CreateSaveToStorageProcessor(IEnergyObservationStorage<IEnergyObservation> observationStorage);

        IEnergyObservationProcessor<IEnergyObservation> CreateSerializeProcessor(Stream stream);
    }

    public interface IStorageFactory<T> where T : IEnergyObservation
    {
        IEnergyObservationStorage<IEnergyObservation> CreateFileStorage(string filePath, SerializeProcessor<IEnergyObservation> serializer);


        IEnergyObservationStorage<IEnergyObservation> CreateListStorage();
    }

    public interface IAnalizerFactory<T> where T: IEnergyObservation
    {
        IEnergyObservationAnalizer<T> CreateAnalizer(IEnergyObservationStorage<IEnergyObservation> observationStorage);
    }
}
