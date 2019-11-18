using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;
using Potestas.Interfaces;
using Potestas.Observations;
using Potestas.Processors.Save;
using System;

namespace Potestas.Tests.Serializer
{
    [TestFixture]
    public class SaveToBsonProcessorTests
    {
        private const string connectionString = @"mongodb://localhost:27017";

        [Test]
        [TestCase(11.2, 14.0, 14.77, 1993)]
        public void SaveToBsonProcessor_SaveToDatabase(double x, double y, double intensity, int duration)
        {
            // Arrange            
            var processor = new SaveToBsonProcessor<IEnergyObservation>(connectionString);
            var observation = new FlashObservation(duration, intensity, new Coordinates(x, y), DateTime.UtcNow);

            // Act
            processor.OnNext(observation);

            // Assert
            var mongoDbClient = new MongoClient(connectionString);
            var mongoDatabase = mongoDbClient.GetDatabase("Observations");
            var bsonCollection = mongoDatabase.GetCollection<BsonDocument>("FlashObservation");

            var bsonDoc = bsonCollection.Find(new BsonDocument()).Sort(Builders<BsonDocument>.Sort.Descending("_id")).FirstOrDefault();

            Assert.NotNull(bsonDoc);

            var observationFromDb = new FlashObservation
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

            Assert.AreEqual(observationFromDb.Intensity, observation.Intensity);
            Assert.AreEqual(observationFromDb.DurationMs, observation.DurationMs);
            Assert.AreEqual(observationFromDb.EstimatedValue, observation.EstimatedValue);
        }
    }
}
