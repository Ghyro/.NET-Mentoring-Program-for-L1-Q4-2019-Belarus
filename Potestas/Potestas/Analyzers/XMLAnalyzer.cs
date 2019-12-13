using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using Potestas.Interfaces;

namespace Potestas.Analyzers
{
    public class XMLAnalyzer<T> : IEnergyObservationAnalyzer<T> where T : IEnergyObservation
    {
        private readonly XDocument _xdoc;
        private const string OBSERVATIONS = "Observations";
        private const string FLASH_OBSERVATIONS = "FlashObservation";
        private const string OBSERVATION_TIME = "ObservationTime";
        private const string ESTIMATED_VALUE = "EstimatedValue";
        private const string OBSERVATION_POINT = "ObservationPoint";

        public XMLAnalyzer()
        {
            _xdoc = XDocument.Load(ConfigurationManager.AppSettings["xmlStoragePath"]);
        }

        public double GetAverageEnergy()
        {
            return _xdoc.Element(OBSERVATIONS).Elements(FLASH_OBSERVATIONS)
                       .Elements(ESTIMATED_VALUE).Average(x => double.Parse(x.Value));
        }

        public double GetAverageEnergy(DateTime startFrom, DateTime endBy)
        {
            if (startFrom == null || endBy == null)
                throw new ArgumentException($"{nameof(startFrom)}, {nameof(endBy)}");

            var resultObservations = _xdoc.Element(OBSERVATIONS).Elements(FLASH_OBSERVATIONS)
                                        .Where(x => (DateTime)x.Element(OBSERVATION_TIME) > startFrom
                                        && (DateTime)x.Element(OBSERVATION_TIME) < endBy)
                                        .ToList();
            return resultObservations.Sum(x => (double)x.Element(ESTIMATED_VALUE)) / resultObservations.Count;
        }

        public double GetAverageEnergy(Coordinates rectTopLeft, Coordinates rectBottomRight)
        {
            if (rectTopLeft == null || rectBottomRight == null)
                throw new ArgumentException($"{nameof(rectTopLeft)}, {nameof(rectBottomRight)}");

            var resultObservations = _xdoc.Element(OBSERVATIONS).Elements(FLASH_OBSERVATIONS)
                                        .Where(x => (double)x.Element(OBSERVATION_POINT).Element("X") > rectTopLeft.X
                                        && (double)x.Element(OBSERVATION_POINT).Element("Y") < rectBottomRight.X
                                        && (double)x.Element(OBSERVATION_POINT).Element("Y") < rectTopLeft.Y
                                        && (double)x.Element(OBSERVATION_POINT).Element("Y") > rectBottomRight.Y).ToList();
            return resultObservations.Sum(x => (double)x.Element(OBSERVATION_POINT)) / resultObservations.Count;
        }

        public IDictionary<Coordinates, int> GetDistributionByCoordinates()
        {
            Coordinates Creator(XElement item) => new Coordinates
            {
                X = (double) item.Element("X"),
                Y = (double) item.Element("Y")
            };

            return _xdoc.Element(OBSERVATIONS).Elements(FLASH_OBSERVATIONS).GroupBy(Creator).ToDictionary(x => x.Key, v => v.Count());
        }

        public IDictionary<double, int> GetDistributionByEnergyValue()
        {
            return _xdoc.Element(OBSERVATIONS).Elements(FLASH_OBSERVATIONS).GroupBy(x => (double)x.Element(ESTIMATED_VALUE)).ToDictionary(x => x.Key, v => v.Count());
        }

        public IDictionary<DateTime, int> GetDistributionByObservationTime()
        {
            return _xdoc.Element(OBSERVATIONS).Elements(FLASH_OBSERVATIONS).GroupBy(x => (DateTime)x.Element(OBSERVATION_TIME)).ToDictionary(x => x.Key, v => v.Count());
        }

        public double GetMaxEnergy()
        {
            return _xdoc.Element(OBSERVATIONS).Elements(FLASH_OBSERVATIONS)
                        .Elements(ESTIMATED_VALUE).Max(x => double.Parse(x.Value));
        }

        public double GetMaxEnergy(Coordinates coordinates)
        {
            if (coordinates == null)
                throw new ArgumentException(nameof(coordinates));

            bool Expression(XElement item)
            {
                var flashCoordinatis = new Coordinates
                {
                    X = (double) item.Element("X"),
                    Y = (double) item.Element("Y")
                };

                return flashCoordinatis == coordinates;
            }

            return _xdoc.Element(OBSERVATIONS).Elements(FLASH_OBSERVATIONS).Elements(OBSERVATION_POINT)
                .Where(Expression).Max(s => (double)s.Element(ESTIMATED_VALUE));
        }

        public double GetMaxEnergy(DateTime dateTime)
        {
            if (dateTime == null)
                throw new ArgumentException(nameof(dateTime));

            return _xdoc.Element(OBSERVATIONS).Elements(FLASH_OBSERVATIONS).Elements(OBSERVATION_POINT)
                .Where(x => (DateTime)x.Element(OBSERVATION_POINT) == dateTime).Max(s => (double)s.Element(ESTIMATED_VALUE));
        }

        public Coordinates GetMaxEnergyPosition()
        {
            var s = _xdoc.Element(OBSERVATIONS).Elements(FLASH_OBSERVATIONS).First(x => Math.Abs((double)x.Element(ESTIMATED_VALUE)
                - _xdoc.Element(OBSERVATIONS).Elements(FLASH_OBSERVATIONS).Max(v => (double)v.Element(ESTIMATED_VALUE))) < 0.001);

            return new Coordinates
            {
                X = (double)s.Element("X"),
                Y = (double)s.Element("Y")
            };
        }

        public DateTime GetMaxEnergyTime()
        {
            return (DateTime)_xdoc.Element(OBSERVATIONS).Elements(FLASH_OBSERVATIONS).First(x => Math.Abs((double)x.Element(ESTIMATED_VALUE)
                - _xdoc.Element(OBSERVATIONS).Elements(FLASH_OBSERVATIONS).Max(v => (double)v.Element(ESTIMATED_VALUE))) < 0.001);
        }

        public double GetMinEnergy()
        {
            return _xdoc.Element(OBSERVATIONS).Elements(FLASH_OBSERVATIONS)
                       .Elements(ESTIMATED_VALUE).Min(x => double.Parse(x.Value));
        }

        public double GetMinEnergy(Coordinates coordinates)
        {
            if (coordinates == null)
                throw new ArgumentException(nameof(coordinates));

            bool Expression(XElement item)
            {
                var flashCoordinatis = new Coordinates
                {
                    X = (double) item.Element("X"),
                    Y = (double) item.Element("Y")
                };

                return flashCoordinatis == coordinates;
            }

            return _xdoc.Element(OBSERVATIONS).Elements(FLASH_OBSERVATIONS).Elements(OBSERVATION_POINT)
                .Where(Expression).Min(s => (double)s.Element(ESTIMATED_VALUE));
        }

        public double GetMinEnergy(DateTime dateTime)
        {
            if (dateTime == null)
                throw new ArgumentException(nameof(dateTime));

            return _xdoc.Element(OBSERVATIONS).Elements(FLASH_OBSERVATIONS).Elements(OBSERVATION_POINT)
                .Where(x => (DateTime)x.Element(OBSERVATION_POINT) == dateTime).Min(s => (double)s.Element(ESTIMATED_VALUE));
        }

        public Coordinates GetMinEnergyPosition()
        {
            var s = _xdoc.Element(OBSERVATIONS).Elements(FLASH_OBSERVATIONS).First(x => Math.Abs((double)x.Element(ESTIMATED_VALUE)
                - _xdoc.Element(OBSERVATIONS).Elements(FLASH_OBSERVATIONS).Min(v => (double)v.Element(ESTIMATED_VALUE))) < 0.001);

            return new Coordinates
            {
                X = (double)s.Element("X"),
                Y = (double)s.Element("Y")
            };
        }

        public DateTime GetMinEnergyTime()
        {
            return (DateTime)_xdoc.Element(OBSERVATIONS).Elements(FLASH_OBSERVATIONS).First(x => Math.Abs((double)x.Element(ESTIMATED_VALUE)
                - _xdoc.Element(OBSERVATIONS).Elements(FLASH_OBSERVATIONS).Min(v => (double)v.Element(ESTIMATED_VALUE))) < 0.001);
        }
    }
}
