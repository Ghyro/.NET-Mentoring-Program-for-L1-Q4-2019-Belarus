using System.Collections.Generic;

namespace Potestas.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Usually repositories are implemented by another way.
    /// </remarks>
    public interface IEnergyObservationStorage : ICollection<IEnergyObservation>
    {
        string Description { get; }
    }
}
