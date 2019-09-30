namespace Potestas.Interfaces

{
    public interface IProcessingGroup
    {
        IEnergyObservationProcessor Processor { get; }

        IEnergyObservationStorage Storage { get; }

        IEnergyObservationAnalizer Analizer { get; }

        void Detach();
    }
}
