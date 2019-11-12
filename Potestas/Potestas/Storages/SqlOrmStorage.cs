using Microsoft.EntityFrameworkCore;
using Potestas.Context;
using Potestas.Interfaces;
using Potestas.Observations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Potestas.Storages
{
    public class SqlOrmStorage<T> : IEnergyObservationStorage<T> where T : IEnergyObservation
    {
        private List<FlashObservation> _observations;
        private ObservationContext _dbContext;

        public SqlOrmStorage(ObservationContext context)
        {
            _dbContext = context;
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

                InsertToDatabaseTable();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public int Count => _observations.Count;

        public bool IsReadOnly => false;

        public string Description => "SqlStorage";

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
            try
            {
                using (_dbContext)
                {
                    var items = _dbContext.FlashObservations.Include(x => x.ObservationPoint).ToList();

                    foreach(var item in items)                    
                        _observations.Add(item);                                                 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void InsertToDatabaseTable()
        {
            using (_dbContext)
            {
                foreach (var flash in _observations)
                {                    
                    AddCoordinatesToDatabase(flash.ObservationPoint);

                    var coordinatesFromDb = GetLastFromDatabase(flash.ObservationPoint).Result;

                    flash.CoordinatesId = coordinatesFromDb.Id;

                    AddObservationToDatabase(flash);
                }
            }
        }

        private void ClearDatabaseTable()
        {
            using (_dbContext)
            {
                _dbContext.Database.ExecuteSqlCommand(@"DELETE FROM Coordinates");
                _dbContext.Database.ExecuteSqlCommand(@"DELETE FROM FlashObservations");
                _dbContext.SaveChanges();
            }
        }

        private void AddCoordinatesToDatabase(Coordinates coordinates)
        {
            _dbContext.Coordinates.Add(coordinates);

            _dbContext.SaveChanges();
        }

        private void AddObservationToDatabase(FlashObservation flashObservation)
        {
            _dbContext.FlashObservations.Add(flashObservation);

            _dbContext.SaveChanges();
        }

        private async Task<Coordinates> GetLastFromDatabase(Coordinates coordinates)
        {
            var item = await _dbContext.Coordinates.LastOrDefaultAsync(x => x.X == coordinates.X && x.Y == coordinates.Y);

            if (item == null)
                throw new ArgumentNullException(nameof(item));

            return item;
        }

        #endregion
    }
}
