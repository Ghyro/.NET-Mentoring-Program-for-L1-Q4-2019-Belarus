using System;

namespace Potestas
{
    /* TASK. Make this interface more usable:
     * 1. Several observations could be compared by ObservationPoint, EstimatedValue and ObservationTime.
     * Implement IEqualityComparer and IComparer for such comparison
     */
    public interface IEnergyObservation
    {
        Coordinates ObservationPoint { get; }

        double EstimatedValue { get; }

        DateTime ObservationTime { get; }
    }
}
