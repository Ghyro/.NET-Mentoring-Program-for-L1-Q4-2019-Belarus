using System.Collections.Generic;
using System.Reflection;

namespace Potestas.Interfaces
{
    public interface IEnergyObservationApplicationModel
    {
        IReadOnlyCollection<ISourceFactory> SourceFactories { get; }

        IReadOnlyCollection<IProcessingFactory> ProcessingFactories { get; }

        IReadOnlyCollection<ISourceRegistration> RegisteredSources { get; }

        void LoadPlugin(Assembly assembly);

        ISourceRegistration CreateAndRegisterSource(ISourceFactory factory);
    }
}
