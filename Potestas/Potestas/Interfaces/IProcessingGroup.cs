namespace Potestas.Interfaces

{
    public interface IProcessingGroup<T> where T: IEnergyObservation
    {
        IEnergyObservationProcessor<T> Processor { get; }

        IEnergyObservationStorage<T> Storage { get; }

        IEnergyObservationAnalizer<T> Analizer { get; }

        void Detach();
    }
}
