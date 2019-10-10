namespace Potestas.Interfaces

{
    public interface IProcessingGroup
    {
        IEnergyObservationProcessor<IEnergyObservation> Processor { get; }

        IEnergyObservationStorage Storage { get; }

        IEnergyObservationAnalizer Analizer { get; }

        void Detach();
    }
}
