using MongoDB.Bson;
using MongoDB.Driver;
using Potestas.Interfaces;
using System;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using Potestas.Observations;
using System.Linq;

namespace Potestas.Analyzers
{
    public class BsonAnalyzer<T> : IEnergyObservationAnalizer<T> where T : IEnergyObservation
    {
        private MongoClient _mongoClient;
        private IMongoDatabase _mongoDatabase;
        private IMongoCollection<BsonDocument> _bsonCollection;

        public BsonAnalyzer(MongoClient mongoClient, string databaseName, string bsonCollectionName)
        {
            _mongoClient = mongoClient;
            _mongoDatabase = _mongoClient.GetDatabase(databaseName);
            _bsonCollection = _mongoDatabase.GetCollection<BsonDocument>(bsonCollectionName);
        }

        public double GetAverageEnergy()
        {
            var bsonElements = _bsonCollection.Aggregate().Group(new BsonDocument { { "_id", "$EstimatedValue" } }).ToList();

            var values = new List<double>();

            foreach (var bsonDoc in bsonElements)
            {
                values.Add(bsonDoc[0].AsDouble);
            }

            if (values == null)
                return 0;

            return values.Average();
        }

        public double GetAverageEnergy(DateTime startFrom, DateTime endBy)
        {
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Gt("ObservationTime", startFrom) & builder.Lt("ObservationTime", endBy);

            var bsonElements = _bsonCollection.Find(filter).ToList();

            var values = new List<double>();

            foreach (var bsonDoc in bsonElements)
            {
                values.Add(bsonDoc["Intensity"].AsDouble);
            }

            if (values == null)
                return 0;

            return values.Average();
        }

        public double GetAverageEnergy(Coordinates rectTopLeft, Coordinates rectBottomRight)
        {
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Gt("Coornidates", rectTopLeft) & builder.Lt("Coornidates", rectBottomRight);

            var bsonElements = _bsonCollection.Find(filter).ToList();

            var values = new List<double>();

            foreach (var bsonDoc in bsonElements)
            {
                values.Add(bsonDoc["Intensity"].AsDouble);
            }

            if (values == null)
                return 0;

            return values.Average();
        }

        public IDictionary<Coordinates, int> GetDistributionByCoordinates()
        {
            var builder = Builders<BsonDocument>.Filter;

            var bsonElements = _bsonCollection.Find(new BsonDocument()).ToList();

            var _observations = new List<FlashObservation>();

            foreach (var bsonDoc in bsonElements)
            {
                var flashObservation = new FlashObservation
                {
                    Intensity = bsonDoc["Intensity"].AsDouble,
                    DurationMs = bsonDoc["DurationMs"].AsInt32,
                    ObservationTime = bsonDoc["ObservationTime"].ToUniversalTime(),
                    EstimatedValue = bsonDoc["EstimatedValue"].AsDouble,
                    ObservationPoint = new Coordinates
                    {
                        X = bsonDoc["Coordinates"]["X"].AsDouble,
                        Y = bsonDoc["Coordinates"]["Y"].AsDouble
                    }
                };

                _observations.Add(flashObservation);
            }

            if (_observations == null)
                throw new ArgumentNullException(nameof(_observations));

            return _observations.GroupBy(x => x.ObservationPoint).ToDictionary(y => y.Key, s => s.Count());
        }

        public IDictionary<double, int> GetDistributionByEnergyValue()
        {
            var builder = Builders<BsonDocument>.Filter;

            var bsonElements = _bsonCollection.Find(new BsonDocument()).ToList();

            var _observations = new List<FlashObservation>();

            foreach (var bsonDoc in bsonElements)
            {
                var flashObservation = new FlashObservation
                {
                    Intensity = bsonDoc["Intensity"].AsDouble,
                    DurationMs = bsonDoc["DurationMs"].AsInt32,
                    ObservationTime = bsonDoc["ObservationTime"].ToUniversalTime(),
                    EstimatedValue = bsonDoc["EstimatedValue"].AsDouble,
                    ObservationPoint = new Coordinates
                    {
                        X = bsonDoc["Coordinates"]["X"].AsDouble,
                        Y = bsonDoc["Coordinates"]["Y"].AsDouble
                    }
                };

                _observations.Add(flashObservation);

            }

            if (_observations == null)
                throw new ArgumentNullException(nameof(_observations));

            return _observations.GroupBy(x => x.EstimatedValue).ToDictionary(y => y.Key, s => s.Count());
        }

        public IDictionary<DateTime, int> GetDistributionByObservationTime()
        {
            var builder = Builders<BsonDocument>.Filter;

            var bsonElements = _bsonCollection.Find(new BsonDocument()).ToList();

            var _observations = new List<FlashObservation>();

            foreach (var bsonDoc in bsonElements)
            {
                var flashObservation = new FlashObservation
                {
                    Intensity = bsonDoc["Intensity"].AsDouble,
                    DurationMs = bsonDoc["DurationMs"].AsInt32,
                    ObservationTime = bsonDoc["ObservationTime"].ToUniversalTime(),
                    EstimatedValue = bsonDoc["EstimatedValue"].AsDouble,
                    ObservationPoint = new Coordinates
                    {
                        X = bsonDoc["Coordinates"]["X"].AsDouble,
                        Y = bsonDoc["Coordinates"]["Y"].AsDouble
                    }
                };

                _observations.Add(flashObservation);
            }

            if (_observations == null)
                throw new ArgumentNullException(nameof(_observations));

            return _observations.GroupBy(x => x.ObservationTime).ToDictionary(y => y.Key, s => s.Count());
        }

        public double GetMaxEnergy()
        {
            var bsonElements = _bsonCollection.Aggregate().Group(new BsonDocument { { "_id", "$EstimatedValue" } }).ToList();

            var values = new List<double>();

            foreach (var bsonDoc in bsonElements)
            {
                values.Add(bsonDoc[0].AsDouble);
            }

            if (values == null)
                return 0;

            return values.Max();
        }

        public double GetMaxEnergy(Coordinates coordinates)
        {
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("Coordinates", coordinates);

            var bsonElements = _bsonCollection.Find(filter).ToList();

            var values = new List<double>();

            foreach (var bsonDoc in bsonElements)
            {
                values.Add(bsonDoc["Intensity"].AsDouble);
            }

            if (values == null)
                return 0;

            return values.Max();
        }

        public double GetMaxEnergy(DateTime dateTime)
        {
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("ObservationTime", dateTime);

            var bsonElements = _bsonCollection.Find(filter).ToList();

            var values = new List<double>();

            foreach (var bsonDoc in bsonElements)
            {
                values.Add(bsonDoc["Intensity"].AsDouble);
            }

            if (values == null)
                return 0;

            return values.Max();
        }

        public Coordinates GetMaxEnergyPosition()
        {
            var builder = Builders<BsonDocument>.Filter;

            var bsonElements = _bsonCollection.Find(new BsonDocument()).ToList();

            var _observations = new List<FlashObservation>();

            foreach (var bsonDoc in bsonElements)
            {
                var flashObservation = new FlashObservation
                {
                    Intensity = bsonDoc["Intensity"].AsDouble,
                    DurationMs = bsonDoc["DurationMs"].AsInt32,
                    ObservationTime = bsonDoc["ObservationTime"].ToUniversalTime(),
                    EstimatedValue = bsonDoc["EstimatedValue"].AsDouble,
                    ObservationPoint = new Coordinates
                    {
                        X = bsonDoc["Coordinates"]["X"].AsDouble,
                        Y = bsonDoc["Coordinates"]["Y"].AsDouble
                    }
                };

                _observations.Add(flashObservation);
            }

            if (_observations == null)
                throw new ArgumentNullException(nameof(_observations));

            return _observations.First(x => Math.Abs(x.EstimatedValue - _observations.Max(v => v.EstimatedValue)) < 0.001).ObservationPoint;
        }

        public DateTime GetMaxEnergyTime()
        {
            var builder = Builders<BsonDocument>.Filter;

            var bsonElements = _bsonCollection.Find(new BsonDocument()).ToList();

            var _observations = new List<FlashObservation>();

            foreach (var bsonDoc in bsonElements)
            {
                var flashObservation = new FlashObservation
                {
                    Intensity = bsonDoc["Intensity"].AsDouble,
                    DurationMs = bsonDoc["DurationMs"].AsInt32,
                    ObservationTime = bsonDoc["ObservationTime"].ToUniversalTime(),
                    EstimatedValue = bsonDoc["EstimatedValue"].AsDouble,
                    ObservationPoint = new Coordinates
                    {
                        X = bsonDoc["Coordinates"]["X"].AsDouble,
                        Y = bsonDoc["Coordinates"]["Y"].AsDouble
                    }
                };

                _observations.Add(flashObservation);
            }

            if (_observations == null)
                throw new ArgumentNullException(nameof(_observations));

            return _observations.First(x => Math.Abs(x.EstimatedValue - _observations.Max(v => v.EstimatedValue)) < 0.001).ObservationTime;
        }

        public double GetMinEnergy()
        {
            var bsonElements = _bsonCollection.Aggregate().Group(new BsonDocument { { "_id", "$EstimatedValue" } }).ToList();

            var values = new List<double>();

            foreach (var bsonDoc in bsonElements)
            {
                values.Add(bsonDoc[0].AsDouble);
            }

            if (values == null)
                return 0;

            return values.Min();
        }

        public double GetMinEnergy(Coordinates coordinates)
        {
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("Coordinates", coordinates);

            var bsonElements = _bsonCollection.Find(filter).ToList();

            var values = new List<double>();

            foreach (var bsonDoc in bsonElements)
            {
                values.Add(bsonDoc["Intensity"].AsDouble);
            }

            if (values == null)
                return 0;

            return values.Min();
        }

        public double GetMinEnergy(DateTime dateTime)
        {
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("ObservationTime", dateTime);

            var bsonElements = _bsonCollection.Find(filter).ToList();

            var values = new List<double>();

            foreach (var bsonDoc in bsonElements)
            {
                values.Add(bsonDoc["Intensity"].AsDouble);
            }

            if (values == null)
                return 0;

            return values.Min();
        }

        public Coordinates GetMinEnergyPosition()
        {
            var builder = Builders<BsonDocument>.Filter;

            var bsonElements = _bsonCollection.Find(new BsonDocument()).ToList();

            var _observations = new List<FlashObservation>();

            foreach (var bsonDoc in bsonElements)
            {
                var flashObservation = new FlashObservation
                {
                    Intensity = bsonDoc["Intensity"].AsDouble,
                    DurationMs = bsonDoc["DurationMs"].AsInt32,
                    ObservationTime = bsonDoc["ObservationTime"].ToUniversalTime(),
                    EstimatedValue = bsonDoc["EstimatedValue"].AsDouble,
                    ObservationPoint = new Coordinates
                    {
                        X = bsonDoc["Coordinates"]["X"].AsDouble,
                        Y = bsonDoc["Coordinates"]["Y"].AsDouble
                    }
                };

                _observations.Add(flashObservation);
            }

            if (_observations == null)
                throw new ArgumentNullException(nameof(_observations));

            return _observations.First(x => Math.Abs(x.EstimatedValue - _observations.Min(v => v.EstimatedValue)) < 0.001).ObservationPoint;
        }

        public DateTime GetMinEnergyTime()
        {
            var builder = Builders<BsonDocument>.Filter;

            var bsonElements = _bsonCollection.Find(new BsonDocument()).ToList();

            var _observations = new List<FlashObservation>();

            foreach (var bsonDoc in bsonElements)
            {
                var flashObservation = new FlashObservation
                {
                    Intensity = bsonDoc["Intensity"].AsDouble,
                    DurationMs = bsonDoc["DurationMs"].AsInt32,
                    ObservationTime = bsonDoc["ObservationTime"].ToUniversalTime(),
                    EstimatedValue = bsonDoc["EstimatedValue"].AsDouble,
                    ObservationPoint = new Coordinates
                    {
                        X = bsonDoc["Coordinates"]["X"].AsDouble,
                        Y = bsonDoc["Coordinates"]["Y"].AsDouble
                    }
                };

                _observations.Add(flashObservation);
            }

            if (_observations == null)
                throw new ArgumentNullException(nameof(_observations));

            return _observations.First(x => Math.Abs(x.EstimatedValue - _observations.Min(v => v.EstimatedValue)) < 0.001).ObservationTime;
        }
    }
}
