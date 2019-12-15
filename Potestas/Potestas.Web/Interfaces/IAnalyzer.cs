using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Potestas.Web.Models;

namespace Potestas.Web.Interfaces
{
    public interface IAnalyzer
    {
        Task<IDictionary<double, int>> GetDistributionByEnergyValueAsync();

        Task<IDictionary<CoordinatesViewModel, int>> GetDistributionByCoordinatesAsync();

        Task<IDictionary<DateTime, int>> GetDistributionByObservationTimeAsync();

        Task<double> GetMaxEnergyAsync();

        Task<double> GetMaxEnergyAsync(CoordinatesViewModel coordinates);

        Task<double> GetMaxEnergyAsync(DateTime dateTime);

        Task<double> GetMinEnergyAsync();

        Task<double> GetMinEnergyAsync(CoordinatesViewModel coordinates);

        Task<double> GetMinEnergyAsync(DateTime dateTime);

        Task<double> GetAverageEnergyAsync();

        Task<double> GetAverageEnergyAsync(DateTime startFrom, DateTime endBy);

        Task<double> GetAverageEnergyAsync(CoordinatesViewModel rectTopLeft, CoordinatesViewModel rectBottomRight);

        Task<DateTime> GetMaxEnergyTimeAsync();

        Task<CoordinatesViewModel> GetMaxEnergyPositionAsync();

        Task<DateTime> GetMinEnergyTimeAsync();

        Task<CoordinatesViewModel> GetMinEnergyPositionAsync();
    }
}
