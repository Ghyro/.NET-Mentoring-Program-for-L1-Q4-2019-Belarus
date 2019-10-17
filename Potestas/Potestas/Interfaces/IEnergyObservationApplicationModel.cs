using System.Collections.Generic;
using System.Reflection;

namespace Potestas.Interfaces
{
    public interface IEnergyObservationApplicationModel
    {
        IReadOnlyCollection<ISourceFactory<IEnergyObservation>> SourceFactories { get; }

        IReadOnlyCollection<IProcessingFactory<IEnergyObservation>> ProcessingFactories { get; }

        IReadOnlyCollection<ISourceRegistration> RegisteredSources { get; }

        void LoadPlugin(Assembly assembly);

        ISourceRegistration CreateAndRegisterSource(ISourceFactory<IEnergyObservation> factory);
    }
}
