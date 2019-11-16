using System;

namespace Potestas.Interfaces
{
    /* TASK. Make this interface more usable:
     * 1. Several observations could be compared by ObservationPoint, EstimatedValue and ObservationTime.
     * Implement IEqualityComparer and IComparer for such comparison
     */
    public interface IEnergyObservation
    {
        int Id { get; set; }

        Coordinates ObservationPoint { get; }

        double EstimatedValue { get; }

        DateTime ObservationTime { get; }
    }
}
