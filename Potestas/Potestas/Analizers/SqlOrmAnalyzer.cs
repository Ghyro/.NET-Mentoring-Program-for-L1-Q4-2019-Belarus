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
            return _dbContext.FlashObservations.Average(x => x.EstimatedValue);
        }

        public double GetAverageEnergy(DateTime startFrom, DateTime endBy)
        {
            var resultObservations = _dbContext.FlashObservations.Where(x => x.ObservationTime > startFrom && x.ObservationTime < endBy).ToList();

            return resultObservations.Sum(x => x.EstimatedValue) / resultObservations.Count();
        }

        public double GetAverageEnergy(Coordinates rectTopLeft, Coordinates rectBottomRight)
        {
            var resultObservations = _dbContext.FlashObservations.Where(x => x.ObservationPoint.X > rectTopLeft.X && x.ObservationPoint.X < rectBottomRight.X
                                                     && x.ObservationPoint.Y < rectTopLeft.Y && x.ObservationPoint.Y > rectBottomRight.Y)
                                                     .ToList();

            return resultObservations.Sum(x => x.EstimatedValue) / resultObservations.Count();
        }

        public IDictionary<Coordinates, int> GetDistributionByCoordinates()
        {
            return _dbContext.FlashObservations.GroupBy(x => x.ObservationPoint).ToDictionary(y => y.Key, s => s.Count());
        }

        public IDictionary<double, int> GetDistributionByEnergyValue()
        {
            return _dbContext.FlashObservations.GroupBy(x => x.EstimatedValue).ToDictionary(y => y.Key, s => s.Count());
        }

        public IDictionary<DateTime, int> GetDistributionByObservationTime()
        {
            return _dbContext.FlashObservations.GroupBy(x => x.ObservationTime).ToDictionary(y => y.Key, s => s.Count());
        }

        public double GetMaxEnergy()
        {
            return _dbContext.FlashObservations.Max(x => x.EstimatedValue);
        }

        public double GetMaxEnergy(Coordinates coordinates)
        {
            return _dbContext.FlashObservations.Where(x => x.ObservationPoint.Equals(coordinates)).Max(s => s.EstimatedValue);
        }

        public double GetMaxEnergy(DateTime dateTime)
        {
            return _dbContext.FlashObservations.Where(x => x.ObservationTime.Equals(dateTime)).Max(s => s.EstimatedValue);
        }

        public Coordinates GetMaxEnergyPosition()
        {
            return _dbContext.FlashObservations.First(x => Math.Abs(x.EstimatedValue - _dbContext.FlashObservations.Max(v => v.EstimatedValue)) < 0.001).ObservationPoint;
        }

        public DateTime GetMaxEnergyTime()
        {
            return _dbContext.FlashObservations.First(x => Math.Abs(x.EstimatedValue - _dbContext.FlashObservations.Max(v => v.EstimatedValue)) < 0.001).ObservationTime;
        }

        public double GetMinEnergy()
        {
            return _dbContext.FlashObservations.Min(x => x.EstimatedValue);
        }

        public double GetMinEnergy(Coordinates coordinates)
        {
            return _dbContext.FlashObservations.Where(x => x.ObservationPoint.Equals(coordinates)).Min(s => s.EstimatedValue);
        }

        public double GetMinEnergy(DateTime dateTime)
        {
            return _dbContext.FlashObservations.Where(x => x.ObservationTime.Equals(dateTime)).Min(s => s.EstimatedValue);
        }

        public Coordinates GetMinEnergyPosition()
        {
            return _dbContext.FlashObservations.First(x => Math.Abs(x.EstimatedValue - _dbContext.FlashObservations.Min(v => v.EstimatedValue)) < 0.001).ObservationPoint;
        }

        public DateTime GetMinEnergyTime()
        {
            return _dbContext.FlashObservations.First(x => Math.Abs(x.EstimatedValue - _dbContext.FlashObservations.Min(v => v.EstimatedValue)) < 0.001).ObservationTime;
        }
    }
}
