using System;
using System.Collections.Generic;

namespace Potestas.Analizers
{
    /* TASK. Implement an Analizer for Observations using LINQ
     */
    public class LINQAnalizer : IEnergyObservationAnalizer
    {
        public double GetAverageEnergy()
        {
            throw new NotImplementedException();
        }

        public double GetAverageEnergy(DateTime startFrom, DateTime endBy)
        {
            throw new NotImplementedException();
        }

        public double GetAverageEnergy(Coordinates rectTopLeft, Coordinates rectBottomRight)
        {
            throw new NotImplementedException();
        }

        public IDictionary<Coordinates, int> GetDistributionByCoordinates()
        {
            throw new NotImplementedException();
        }

        public IDictionary<double, int> GetDistributionByEnergyValue()
        {
            throw new NotImplementedException();
        }

        public IDictionary<DateTime, int> GetDistributionByObservationTime()
        {
            throw new NotImplementedException();
        }

        public double GetMaxEnergy()
        {
            throw new NotImplementedException();
        }

        public double GetMaxEnergy(Coordinates coordinates)
        {
            throw new NotImplementedException();
        }

        public double GetMaxEnergy(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public Coordinates GetMaxEnergyPosition()
        {
            throw new NotImplementedException();
        }

        public DateTime GetMaxEnergyTime()
        {
            throw new NotImplementedException();
        }

        public double GetMinEnergy()
        {
            throw new NotImplementedException();
        }

        public double GetMinEnergy(Coordinates coordinates)
        {
            throw new NotImplementedException();
        }

        public double GetMinEnergy(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public Coordinates GetMinEnergyPosition()
        {
            throw new NotImplementedException();
        }

        public DateTime GetMinEnergyTime()
        {
            throw new NotImplementedException();
        }
    }
}
