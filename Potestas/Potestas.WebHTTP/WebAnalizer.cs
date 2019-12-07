using AutoMapper;
using Newtonsoft.Json;
using Potestas.Interfaces;
using Potestas.Observations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Potestas.WebHTTP
{
    public class WebAnalizer<T> : IEnergyObservationAnalizer<T> where T : IEnergyObservation
    {
        private readonly string _url = "http://localhost:6836/api/flashobservation";
        private readonly IMapper mapper = MapperConfig.CreateMapper();
        private readonly IEnumerable<FlashObservation> _observations;

        public WebAnalizer()
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, $"{_url}/get"))
            {
                var response = client.SendAsync(request).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    _observations = mapper.Map<List<FlashObservation>>(
                        JsonConvert.DeserializeObject<List<Models.FlashObservation>>(response.Content.ReadAsStringAsync().Result));
                }
                else
                {
                    _observations = new List<FlashObservation>();
                }
            }
        }

        public double GetAverageEnergy()
        {
            return _observations.Average(x => x.EstimatedValue);
        }

        public double GetAverageEnergy(DateTime startFrom, DateTime endBy)
        {
            var resultObservations = _observations.Where(x => x.ObservationTime > startFrom && x.ObservationTime < endBy).ToList();

            return resultObservations.Sum(x => x.EstimatedValue) / resultObservations.Count();
        }

        public double GetAverageEnergy(Coordinates rectTopLeft, Coordinates rectBottomRight)
        {
            var resultObservations = _observations.Where(x => x.ObservationPoint.X > rectTopLeft.X && x.ObservationPoint.X < rectBottomRight.X
                                                     && x.ObservationPoint.Y < rectTopLeft.Y && x.ObservationPoint.Y > rectBottomRight.Y)
                                                     .ToList();

            return resultObservations.Sum(x => x.EstimatedValue) / resultObservations.Count();
        }

        public IDictionary<Coordinates, int> GetDistributionByCoordinates()
        {
            return _observations.GroupBy(x => x.ObservationPoint).ToDictionary(y => y.Key, s => s.Count());
        }

        public IDictionary<double, int> GetDistributionByEnergyValue()
        {
            return _observations.GroupBy(x => x.EstimatedValue).ToDictionary(y => y.Key, s => s.Count());
        }

        public IDictionary<DateTime, int> GetDistributionByObservationTime()
        {
            return _observations.GroupBy(x => x.ObservationTime).ToDictionary(y => y.Key, s => s.Count());
        }

        public double GetMaxEnergy()
        {
            return _observations.Max(x => x.EstimatedValue);
        }

        public double GetMaxEnergy(Coordinates coordinates)
        {
            return _observations.Where(x => x.ObservationPoint.Equals(coordinates)).Max(s => s.EstimatedValue);
        }

        public double GetMaxEnergy(DateTime dateTime)
        {
            return _observations.Where(x => x.ObservationTime.Equals(dateTime)).Max(s => s.EstimatedValue);
        }

        public Coordinates GetMaxEnergyPosition()
        {
            return _observations.First(x => Math.Abs(x.EstimatedValue - _observations.Max(v => v.EstimatedValue)) < 0.001).ObservationPoint;
        }

        public DateTime GetMaxEnergyTime()
        {
            return _observations.First(x => Math.Abs(x.EstimatedValue - _observations.Max(v => v.EstimatedValue)) < 0.001).ObservationTime;
        }

        public double GetMinEnergy()
        {
            return _observations.Min(x => x.EstimatedValue);
        }

        public double GetMinEnergy(Coordinates coordinates)
        {
            return _observations.Where(x => x.ObservationPoint.Equals(coordinates)).Min(s => s.EstimatedValue);
        }

        public double GetMinEnergy(DateTime dateTime)
        {
            return _observations.Where(x => x.ObservationTime.Equals(dateTime)).Min(s => s.EstimatedValue);
        }

        public Coordinates GetMinEnergyPosition()
        {
            return _observations.First(x => Math.Abs(x.EstimatedValue - _observations.Min(v => v.EstimatedValue)) < 0.001).ObservationPoint;
        }

        public DateTime GetMinEnergyTime()
        {
            return _observations.First(x => Math.Abs(x.EstimatedValue - _observations.Min(v => v.EstimatedValue)) < 0.001).ObservationTime;
        }

    }
}
