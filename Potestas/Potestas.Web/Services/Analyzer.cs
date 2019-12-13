using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Potestas.Interfaces;
using Potestas.Web.Interfaces;
using Potestas.Web.Models;

namespace Potestas.Web.Services
{
    public class Analyzer : IAnalyzer
    {
        private readonly IEnergyObservationAnalyzer<IEnergyObservation> _analyzer;
        private readonly IMapper _mapper;

        public Analyzer(IEnergyObservationAnalyzer<IEnergyObservation> analizer, IMapper mapper)
        {
            _analyzer = analizer;
            _mapper = mapper;
        }

        public async Task<double> GetAverageEnergyAsync() => await Task.Run(() => _analyzer.GetAverageEnergy());

        public async Task<double> GetAverageEnergyAsync(DateTime startFrom, DateTime endBy) => await Task.Run(() => _analyzer.GetAverageEnergy(startFrom, endBy));

        public async Task<double> GetAverageEnergyAsync(CoordinatesViewModel firstCoordinate, CoordinatesViewModel secondCoordinate) => await Task.Run(() => _analyzer
        .GetAverageEnergy(_mapper.Map<Coordinates>(firstCoordinate), _mapper.Map<Coordinates>(secondCoordinate)));

        public async Task<IDictionary<CoordinatesViewModel, int>> GetDistributionByCoordinatesAsync() => await Task.Run(() => _analyzer.GetDistributionByCoordinates()
        .ToDictionary(keyValue => _mapper.Map<CoordinatesViewModel>(keyValue.Key), keyValue => keyValue.Value));

        public async Task<IDictionary<double, int>> GetDistributionByEnergyValueAsync() => await Task.Run(() => _analyzer.GetDistributionByEnergyValue());

        public async Task<IDictionary<DateTime, int>> GetDistributionByObservationTimeAsync() => await Task.Run(() => _analyzer.GetDistributionByObservationTime());

        public async Task<double> GetMaxEnergyAsync() => await Task.Run(() => _analyzer.GetMaxEnergy());

        public async Task<double> GetMaxEnergyAsync(CoordinatesViewModel coordinate) => await Task.Run(() => _analyzer.GetMaxEnergy(_mapper.Map<Coordinates>(coordinate)));

        public async Task<double> GetMaxEnergyAsync(DateTime dateTime) => await Task.Run(() => _analyzer.GetMaxEnergy(dateTime));

        public async Task<CoordinatesViewModel> GetMaxEnergyPositionAsync() => _mapper.Map<CoordinatesViewModel>(await Task.Run(() => _analyzer.GetMaxEnergyPosition()));

        public async Task<DateTime> GetMaxEnergyTimeAsync() => await Task.Run(() => _analyzer.GetMaxEnergyTime());

        public async Task<double> GetMinEnergyAsync() => await Task.Run(() => _analyzer.GetMinEnergy());

        public async Task<double> GetMinEnergyAsync(CoordinatesViewModel coordinate) => await Task.Run(() => _analyzer.GetMinEnergy(_mapper.Map<Coordinates>(coordinate)));

        public async Task<double> GetMinEnergyAsync(DateTime dateTime) => await Task.Run(() => _analyzer.GetMinEnergy(dateTime));

        public async Task<CoordinatesViewModel> GetMinEnergyPositionAsync() => _mapper.Map<CoordinatesViewModel>(await Task.Run(() => _analyzer.GetMinEnergyPosition()));

        public async Task<DateTime> GetMinEnergyTimeAsync() => await Task.Run(() => _analyzer.GetMinEnergyTime());
    }
}
