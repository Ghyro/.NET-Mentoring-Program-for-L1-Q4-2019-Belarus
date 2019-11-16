using Potestas.Context;
using Potestas.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Potestas.Analizers
{
    public class SqlOrmAnalyzer<T> : IEnergyObservationAnalizer<T> where T : IEnergyObservation
    {
        private ObservationContext _dbContext;

        public SqlOrmAnalyzer(ObservationContext context)
        {
            _dbContext = context;
        }

        public double GetAverageEnergy()
        {
            return _dbContext.FlashObservationWrapper.Average(x => x.EstimatedValue);
        }

        public double GetAverageEnergy(DateTime startFrom, DateTime endBy)
        {
            var resultObservations = _dbContext.FlashObservationWrapper.Where(x => x.ObservationTime > startFrom && x.ObservationTime < endBy).ToList();

            return resultObservations.Sum(x => x.EstimatedValue) / resultObservations.Count();
        }

        public double GetAverageEnergy(Coordinates rectTopLeft, Coordinates rectBottomRight)
        {
            var resultObservations = _dbContext.FlashObservationWrapper.Where(x => x.ObservationPoint.X > rectTopLeft.X && x.ObservationPoint.X < rectBottomRight.X
                                                     && x.ObservationPoint.Y < rectTopLeft.Y && x.ObservationPoint.Y > rectBottomRight.Y)
                                                     .ToList();

            return resultObservations.Sum(x => x.EstimatedValue) / resultObservations.Count();
        }

        public IDictionary<Coordinates, int> GetDistributionByCoordinates()
        {
            var result = from f in _dbContext.FlashObservationWrapper
                         join c in _dbContext.CoordinatesWrapper on f.Id equals c.Id
                         group f by f.ObservationPoint;

            throw new NotImplementedException();

            //return result.ToDictionary(x => x.Key, s => s.Count());
        }

        public IDictionary<double, int> GetDistributionByEnergyValue()
        {
            return _dbContext.FlashObservationWrapper.GroupBy(x => x.EstimatedValue).ToDictionary(y => y.Key, s => s.Count());
        }

        public IDictionary<DateTime, int> GetDistributionByObservationTime()
        {
            return _dbContext.FlashObservationWrapper.GroupBy(x => x.ObservationTime).ToDictionary(y => y.Key, s => s.Count());
        }

        public double GetMaxEnergy()
        {
            return _dbContext.FlashObservationWrapper.Max(x => x.EstimatedValue);
        }

        public double GetMaxEnergy(Coordinates coordinates)
        {
            return _dbContext.FlashObservationWrapper.Where(x => x.ObservationPoint.Equals(coordinates)).Max(s => s.EstimatedValue);
        }

        public double GetMaxEnergy(DateTime dateTime)
        {
            return _dbContext.FlashObservationWrapper.Where(x => x.ObservationTime.Equals(dateTime)).Max(s => s.EstimatedValue);
        }

        public Coordinates GetMaxEnergyPosition()
        {
            var result = _dbContext.FlashObservationWrapper.First(x => Math.Abs(x.EstimatedValue - _dbContext.FlashObservationWrapper.Max(v => v.EstimatedValue)) < 0.001).ObservationPoint;

            return new Coordinates
            {
                Id = result.Id,
                X = result.X,
                Y = result.Y
            };
        }

        public DateTime GetMaxEnergyTime()
        {
            return _dbContext.FlashObservationWrapper.First(x => Math.Abs(x.EstimatedValue - _dbContext.FlashObservationWrapper.Max(v => v.EstimatedValue)) < 0.001).ObservationTime;
        }

        public double GetMinEnergy()
        {
            return _dbContext.FlashObservationWrapper.Min(x => x.EstimatedValue);
        }

        public double GetMinEnergy(Coordinates coordinates)
        {
            return _dbContext.FlashObservationWrapper.Where(x => x.ObservationPoint.Equals(coordinates)).Min(s => s.EstimatedValue);
        }

        public double GetMinEnergy(DateTime dateTime)
        {
            return _dbContext.FlashObservationWrapper.Where(x => x.ObservationTime.Equals(dateTime)).Min(s => s.EstimatedValue);
        }

        public Coordinates GetMinEnergyPosition()
        {
            var result = _dbContext.FlashObservationWrapper.First(x => Math.Abs(x.EstimatedValue - _dbContext.FlashObservationWrapper.Min(v => v.EstimatedValue)) < 0.001).ObservationPoint;

            return new Coordinates
            {
                Id = result.Id,
                X = result.X,
                Y = result.Y
            };
        }

        public DateTime GetMinEnergyTime()
        {
            return _dbContext.FlashObservationWrapper.First(x => Math.Abs(x.EstimatedValue - _dbContext.FlashObservationWrapper.Min(v => v.EstimatedValue)) < 0.001).ObservationTime;
        }
    }
}
