using System.Collections.Generic;

namespace Potestas
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
