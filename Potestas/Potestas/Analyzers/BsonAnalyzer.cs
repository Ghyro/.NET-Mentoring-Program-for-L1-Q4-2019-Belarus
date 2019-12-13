using MongoDB.Bson;
using MongoDB.Driver;
using Potestas.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using Potestas.Observations;
using System.Linq;

namespace Potestas.Analyzers
{
    public class BsonAnalyzer<T> : IEnergyObservationAnalyzer<T> where T : IEnergyObservation
    {
        private readonly MongoClient _mongoClient;
        private readonly IMongoDatabase _mongoDatabase;
        private readonly IMongoCollection<BsonDocument> _bsonCollection;

        public BsonAnalyzer(MongoClient mongoClient, string databaseName, string bsonCollectionName)
        {
            _mongoClient = mongoClient;
            _mongoDatabase = _mongoClient.GetDatabase(databaseName);
            _bsonCollection = _mongoDatabase.GetCollection<BsonDocument>(bsonCollectionName);
        }

        public BsonAnalyzer()
        {
            _mongoClient = new MongoClient(ConfigurationManager.AppSettings["MongoDbConnection"] ?? "mongodb://localhost:27017");
            _mongoDatabase = _mongoClient.GetDatabase(ConfigurationManager.AppSettings["MongoDbName"] ?? "Observations");
            _bsonCollection = _mongoDatabase.GetCollection<BsonDocument>(ConfigurationManager.AppSettings["MongoDbTable"] ?? "FlashObservation");
        }

        public double GetAverageEnergy()
        {
            var bsonElements = _bsonCollection.Aggregate().Group(new BsonDocument { { "_id", "$EstimatedValue" } }).ToList();

            var values = bsonElements.Select(bsonDoc => bsonDoc[0].AsDouble).ToList();

            return values.Average();
        }

        public double GetAverageEnergy(DateTime startFrom, DateTime endBy)
        {
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Gt("ObservationTime", startFrom) & builder.Lt("ObservationTime", endBy);

            var bsonElements = _bsonCollection.Find(filter).ToList();

            var values = bsonElements.Select(bsonDoc => bsonDoc["Intensity"].AsDouble).ToList();

            return values.Average();
        }

        public double GetAverageEnergy(Coordinates rectTopLeft, Coordinates rectBottomRight)
        {
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Gt("Coornidates", rectTopLeft) & builder.Lt("Coornidates", rectBottomRight);

            var bsonElements = _bsonCollection.Find(filter).ToList();

            var values = bsonElements.Select(bsonDoc => bsonDoc["Intensity"].AsDouble).ToList();

            return values.Average();
        }

        public IDictionary<Coordinates, int> GetDistributionByCoordinates()
        {
            var bsonElements = _bsonCollection.Find(new BsonDocument()).ToList();

            var observations = bsonElements.Select(bsonDoc => new FlashObservation
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
                })
                .ToList();

            if (observations == null)
                throw new ArgumentNullException(nameof(observations));

            return observations.GroupBy(x => x.ObservationPoint).ToDictionary(y => y.Key, s => s.Count());
        }

        public IDictionary<double, int> GetDistributionByEnergyValue()
        {
            var bsonElements = _bsonCollection.Find(new BsonDocument()).ToList();

            var observations = bsonElements.Select(bsonDoc => new FlashObservation
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
                })
                .ToList();

            if (observations == null)
                throw new ArgumentNullException(nameof(observations));

            return observations.GroupBy(x => x.EstimatedValue).ToDictionary(y => y.Key, s => s.Count());
        }

        public IDictionary<DateTime, int> GetDistributionByObservationTime()
        {
            var bsonElements = _bsonCollection.Find(new BsonDocument()).ToList();

            var observations = bsonElements.Select(bsonDoc => new FlashObservation
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
                })
                .ToList();

            if (observations == null)
                throw new ArgumentNullException(nameof(observations));

            return observations.GroupBy(x => x.ObservationTime).ToDictionary(y => y.Key, s => s.Count());
        }

        public double GetMaxEnergy()
        {
            var bsonElements = _bsonCollection.Aggregate().Group(new BsonDocument { { "_id", "$EstimatedValue" } }).ToList();

            var values = bsonElements.Select(bsonDoc => bsonDoc[0].AsDouble).ToList();

            return values.Max();
        }

        public double GetMaxEnergy(Coordinates coordinates)
        {
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("Coordinates", coordinates);

            var bsonElements = _bsonCollection.Find(filter).ToList();

            var values = bsonElements.Select(bsonDoc => bsonDoc["Intensity"].AsDouble).ToList();

            return values.Max();
        }

        public double GetMaxEnergy(DateTime dateTime)
        {
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("ObservationTime", dateTime);

            var bsonElements = _bsonCollection.Find(filter).ToList();

            var values = bsonElements.Select(bsonDoc => bsonDoc["Intensity"].AsDouble).ToList();

            return values.Max();
        }

        public Coordinates GetMaxEnergyPosition()
        {
            var bsonElements = _bsonCollection.Find(new BsonDocument()).ToList();

            var observations = bsonElements.Select(bsonDoc => new FlashObservation
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
                })
                .ToList();

            if (observations == null)
                throw new ArgumentNullException(nameof(observations));

            return observations.First(x => Math.Abs(x.EstimatedValue - observations.Max(v => v.EstimatedValue)) < 0.001).ObservationPoint;
        }

        public DateTime GetMaxEnergyTime()
        {
            var bsonElements = _bsonCollection.Find(new BsonDocument()).ToList();

            var observations = bsonElements.Select(bsonDoc => new FlashObservation
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
                })
                .ToList();

            if (observations == null)
                throw new ArgumentNullException(nameof(observations));

            return observations.First(x => Math.Abs(x.EstimatedValue - observations.Max(v => v.EstimatedValue)) < 0.001).ObservationTime;
        }

        public double GetMinEnergy()
        {
            var bsonElements = _bsonCollection.Aggregate().Group(new BsonDocument { { "_id", "$EstimatedValue" } }).ToList();

            var values = bsonElements.Select(bsonDoc => bsonDoc[0].AsDouble).ToList();

            return values.Min();
        }

        public double GetMinEnergy(Coordinates coordinates)
        {
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("Coordinates", coordinates);

            var bsonElements = _bsonCollection.Find(filter).ToList();

            var values = bsonElements.Select(bsonDoc => bsonDoc["Intensity"].AsDouble).ToList();

            return values.Min();
        }

        public double GetMinEnergy(DateTime dateTime)
        {
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("ObservationTime", dateTime);

            var bsonElements = _bsonCollection.Find(filter).ToList();

            var values = bsonElements.Select(bsonDoc => bsonDoc["Intensity"].AsDouble).ToList();

            return values.Min();
        }

        public Coordinates GetMinEnergyPosition()
        {
            var bsonElements = _bsonCollection.Find(new BsonDocument()).ToList();

            var observations = bsonElements.Select(bsonDoc => new FlashObservation
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
                })
                .ToList();

            if (observations == null)
                throw new ArgumentNullException(nameof(observations));

            return observations.First(x => Math.Abs(x.EstimatedValue - observations.Min(v => v.EstimatedValue)) < 0.001).ObservationPoint;
        }

        public DateTime GetMinEnergyTime()
        {
            var bsonElements = _bsonCollection.Find(new BsonDocument()).ToList();

            var observations = bsonElements.Select(bsonDoc => new FlashObservation
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
                })
                .ToList();

            if (observations == null)
                throw new ArgumentNullException(nameof(observations));

            return observations.First(x => Math.Abs(x.EstimatedValue - observations.Min(v => v.EstimatedValue)) < 0.001).ObservationTime;
        }
    }
}
