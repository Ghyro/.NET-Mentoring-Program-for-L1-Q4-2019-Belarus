using Potestas.Interfaces;
using System;
using MongoDB.Bson;
using MongoDB.Driver;
using Potestas.Observations;
using System.Configuration;

namespace Potestas.Processors.Save
{
    public class SaveToBsonProcessor<T> : IEnergyObservationProcessor<T> where T : IEnergyObservation
    {
        private readonly string _connectionString;

        public SaveToBsonProcessor() { }
        public SaveToBsonProcessor(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string Description => "SaveToBsonProcessor";

        public void OnCompleted()
        {
            Console.WriteLine("SaveToBsonProcessor has completed");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(error.Message);
        }

        public void OnNext(T value)
        {
            if (ReferenceEquals(value, null))
                throw new ArgumentNullException(nameof(T));

            var item = (FlashObservation)(object)value;

            if (ReferenceEquals(item, null))
                throw new ArgumentNullException(nameof(item));

            try
            {
                var bsonDocument = new BsonDocument
                {
                    { "Intensity", item.Intensity },
                    { "DurationMs", item.DurationMs },
                    { "ObservationTime", item.ObservationTime },
                    { "EstimatedValue", item.EstimatedValue },
                    { "Coordinates", new BsonDocument{ { "X", item.ObservationPoint.X }, { "Y", item.ObservationPoint.Y } } },
                };

                var mongoDbClient = new MongoClient(_connectionString ?? ConfigurationManager.AppSettings["MongoDbConnection"]);

                var mongoDatabase = mongoDbClient.GetDatabase("Observations");

                var bsonCollection = mongoDatabase.GetCollection<BsonDocument>("FlashObservation");

                bsonCollection.InsertOne(bsonDocument);

                // It's possible to use standard .NET classes
                //var bsonCollection_standart = mongoDatabase.GetCollection<FlashObservation>("FlashObservation");
                //bsonCollection_standart.InsertOne(item);
            }
            catch(Exception ex)
            {
                OnError(ex);
            }            
        }
    }
}
