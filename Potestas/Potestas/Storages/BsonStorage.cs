using MongoDB.Bson;
using MongoDB.Driver;
using Potestas.Interfaces;
using Potestas.Observations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Potestas.Storages
{
    public class BsonStorage<T> : IEnergyObservationStorage<T> where T : IEnergyObservation
    {
        private readonly List<FlashObservation> _observations;

        public BsonStorage()
        {
            _observations = new List<FlashObservation>();
            FetchFromDatabaseTable();
        }
        public IEnumerator<T> GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _observations.GetEnumerator();
        }

        public void Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _observations.Add((FlashObservation)(object)item);
            InsertToDatabaseTable();
        }

        public void Clear()
        {
            _observations.Clear();
            ClearDatabaseTable();
        }

        public bool Contains(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            return _observations.Contains((FlashObservation)(object)item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (arrayIndex > _observations.Count)
            {
                _observations.AddRange((FlashObservation[])(object)array);
            }
            else
            {
                if (arrayIndex < 0)
                    arrayIndex = 0;
                _observations.InsertRange(arrayIndex, (FlashObservation[])(object)array);
            }

            InsertToDatabaseTable();
        }

        public bool Remove(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            try
            {
                var removeItem = GetByHash(item.GetHashCode());

                if (removeItem == null)
                    throw new ArgumentNullException(nameof(item));

                _observations.Remove((FlashObservation)(object)removeItem);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public int Count => _observations.Count;

        public bool IsReadOnly => false;

        public string Description => "BsonStorage";

        public IEnumerable<T> GetAll()
        {
            return (IEnumerable<T>)_observations;
        }

        public T GetByHash(int hashCode)
        {
            return (T)(object)_observations.SingleOrDefault(item => item.GetHashCode() == hashCode);
        }

        #region private

        private void FetchFromDatabaseTable()
        {
            var mongoDbClient = new MongoClient(ConfigurationManager.AppSettings["MongoDbConnection"] ?? "mongodb://localhost:27017");
            var mongoDb = mongoDbClient.GetDatabase("Observations");
            var mongoCollection = mongoDb.GetCollection<BsonDocument>("FlashObservation");
            var mongoFilter = new BsonDocument();

            try
            {
                var bsonDocuments = mongoCollection.Find(mongoFilter).ToList();

                foreach(var bsonDoc in bsonDocuments)
                {
                    try
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
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void InsertToDatabaseTable()
        {
            var mongoDbClient = new MongoClient(ConfigurationManager.AppSettings["MongoDbConnection"]);

            var mongoDatabase = mongoDbClient.GetDatabase("Observations");

            var bsonCollection = mongoDatabase.GetCollection<BsonDocument>("FlashObservation");

            foreach(var item in _observations)
            {
                var bsonDocument = new BsonDocument
                {
                    { "Intensity", item.Intensity },
                    { "DurationMs", item.DurationMs },
                    { "ObservationTime", item.ObservationTime },
                    { "EstimatedValue", item.EstimatedValue },
                    { "Coordinates", new BsonDocument{ { "X", item.ObservationPoint.X }, { "Y", item.ObservationPoint.Y } } },
                };

                bsonCollection.InsertOne(bsonDocument);
            }
        }

        private void ClearDatabaseTable()
        {
            var mongoDbClient = new MongoClient(ConfigurationManager.AppSettings["MongoDbConnection"] ?? "mongodb://localhost:27017");

            var mongoDatabase = mongoDbClient.GetDatabase("Observations");

            var bsonCollection = mongoDatabase.GetCollection<BsonDocument>("FlashObservation");

            var mongoFilter = new BsonDocument();

            bsonCollection.DeleteMany(mongoFilter);
        }

        public async Task<IEnumerable<FlashObservation>> GetObservations() => _observations;

        #endregion
    }
}
