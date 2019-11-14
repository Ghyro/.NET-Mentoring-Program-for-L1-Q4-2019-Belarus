using Microsoft.EntityFrameworkCore;
using Potestas.Context;
using Potestas.Interfaces;
using Potestas.Observations;
using Potestas.Observations.Wrappers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Potestas.Storages
{
    public class SqlOrmStorage<T> : IEnergyObservationStorage<T> where T : IEnergyObservation
    {
        private List<FlashObservationWrapper> _observations;
        private ObservationContext _dbContext;

        public SqlOrmStorage(ObservationContext context)
        {
            _dbContext = context;
            _observations = new List<FlashObservationWrapper>();
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

            _observations.Add(new FlashObservationWrapper((FlashObservation)(object)item));
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

            return _observations.Contains(new FlashObservationWrapper((FlashObservation)(object)item));
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (arrayIndex > _observations.Count)
            {
                var wrappArray = CreateFlashObservationWrapperArray(array);

                _observations.AddRange(wrappArray);
            }
            else
            {
                if (arrayIndex < 0)
                    arrayIndex = 0;

                var wrappArray = CreateFlashObservationWrapperArray(array);

                _observations.InsertRange(arrayIndex, wrappArray);
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

                _observations.Remove(new FlashObservationWrapper((FlashObservation)(object)removeItem));

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
                    var items = _dbContext.FlashObservationWrapper.Include(x => x.ObservationPoint).ToList();

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
                    var coordinatesWrapper = new CoordinatesWrapper(flash.ObservationPoint);

                    AddCoordinatesToDatabase(coordinatesWrapper);

                    var coordinatesFromDb = GetLastFromDatabase(coordinatesWrapper).Result;

                    var coordinatesId = flash.CoordinatesId;

                    coordinatesId = coordinatesFromDb.Id;

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

        private void AddCoordinatesToDatabase(CoordinatesWrapper coordinates)
        {
            _dbContext.CoordinatesWrapper.Add(coordinates);

            _dbContext.SaveChanges();
        }

        private void AddObservationToDatabase(FlashObservationWrapper flashObservation)
        {
            _dbContext.FlashObservationWrapper.Add(flashObservation);

            _dbContext.SaveChanges();
        }

        private async Task<CoordinatesWrapper> GetLastFromDatabase(CoordinatesWrapper coordinates)
        {
            var item = await _dbContext.CoordinatesWrapper.LastOrDefaultAsync(x => x.X == coordinates.X
                                                                                && x.Y == coordinates.Y);
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            return item;
        }

        private FlashObservationWrapper[] CreateFlashObservationWrapperArray(T[] array)
        {
            var wrappArray = new FlashObservationWrapper[] { };

            for (var i = 0; i < array.Length; i++)
            {
                wrappArray[i] = (FlashObservationWrapper)(object)array[i];
            }

            return wrappArray;
        }

        #endregion
    }
}
