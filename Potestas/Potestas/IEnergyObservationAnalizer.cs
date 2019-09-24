using System;
using System.Collections.Generic;

namespace Potestas
{
    public interface IEnergyObservationAnalizer
    {
        IDictionary<double, int> GetDistributionByEnergyValue();

        IDictionary<Coordinates, int> GetDistributionByCoordinates();

        IDictionary<DateTime, int> GetDistributionByObservationTime();

        double GetMaxEnergy();

        double GetMaxEnergy(Coordinates coordinates);

        double GetMaxEnergy(DateTime dateTime);

        double GetMinEnergy();

        double GetMinEnergy(Coordinates coordinates);

        double GetMinEnergy(DateTime dateTime);

        double GetAverageEnergy();

        double GetAverageEnergy(DateTime startFrom, DateTime endBy);

        double GetAverageEnergy(Coordinates rectTopLeft, Coordinates rectBottomRight);

        DateTime GetMaxEnergyTime();

        Coordinates GetMaxEnergyPosition();

        DateTime GetMinEnergyTime();

        Coordinates GetMinEnergyPosition();
    }
}
