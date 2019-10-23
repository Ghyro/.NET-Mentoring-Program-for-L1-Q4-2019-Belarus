using System.Collections.Generic;
using Potestas.Interfaces;

namespace Potestas.Comparators
{
    public class EnergyObservationComparer : IComparer<IEnergyObservation>
    {
        public int Compare(IEnergyObservation x, IEnergyObservation y)
        {
            if (x == null || y == null)
                return 0;

            if (x.ObservationPoint.X.CompareTo(y.ObservationPoint.X) != 0)
            {
                return x.ObservationPoint.X.CompareTo(y.ObservationPoint.X);
            }
            if (x.ObservationPoint.Y.CompareTo(y.ObservationPoint.Y) != 0)
            {
                return x.ObservationPoint.Y.CompareTo(y.ObservationPoint.Y);
            }
            if (x.EstimatedValue.CompareTo(y.EstimatedValue) != 0)
            {
                return x.EstimatedValue.CompareTo(y.EstimatedValue);
            }
            return x.ObservationTime.CompareTo(y.ObservationTime) != 0 ? x.ObservationTime.CompareTo(y.ObservationTime) : 0;
        }
    }
}
