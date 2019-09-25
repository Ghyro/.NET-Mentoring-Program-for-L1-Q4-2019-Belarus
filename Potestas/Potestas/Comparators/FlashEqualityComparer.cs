using System.Collections.Generic;

namespace Potestas.Comparators
{
    public class FlashEqualityComparer : IEqualityComparer<IEnergyObservation>
    {
        public bool Equals(IEnergyObservation x, IEnergyObservation y)
        {
            if (x == null || y == null)
                return false;

            return x.ObservationPoint.Equals(y.ObservationPoint) && x.EstimatedValue.Equals(y.EstimatedValue) && x.ObservationTime.Equals(y.ObservationTime);
        }

        public int GetHashCode(IEnergyObservation obj)
        {
            unchecked
            {
                var hashCode = obj.ObservationPoint.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.EstimatedValue.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.ObservationTime.GetHashCode();
                return hashCode;
            }
        }
    }
}
