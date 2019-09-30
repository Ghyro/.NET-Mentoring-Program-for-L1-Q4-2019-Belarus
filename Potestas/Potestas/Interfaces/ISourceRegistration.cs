using System.Collections.Generic;
using System.Threading.Tasks;

namespace Potestas.Interfaces
{
    public interface ISourceRegistration
    {
        SourceStatus Status { get; }

        IReadOnlyCollection<IProcessingGroup> ProcessingUnits { get; }

        Task Start();

        void Stop();

        void Unregister();

        IProcessingGroup AttachProcessingGroup(IProcessingFactory factory);
    }
}
