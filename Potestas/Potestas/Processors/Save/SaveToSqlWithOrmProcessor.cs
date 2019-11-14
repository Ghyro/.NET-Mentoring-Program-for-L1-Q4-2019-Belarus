using Microsoft.EntityFrameworkCore;
using Potestas.Context;
using Potestas.Interfaces;
using Potestas.Observations;
using Potestas.Observations.Wrappers;
using System;
using System.Threading.Tasks;

namespace Potestas.Processors.Save
{
    public class SaveToSqlWithOrmProcessor<T> : IEnergyObservationProcessor<T> where T : IEnergyObservation
    {
        private ObservationContext _dbContext;
        public string Description => "SaveToSqlWithOrmProcessor";

        public SaveToSqlWithOrmProcessor(ObservationContext context)
        {
            _dbContext = context;
        }

        public void OnCompleted()
        {
            Console.WriteLine("SaveToSqlWithOrmProcessor has completed");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(error.Message);
        }

        public void OnNext(T value)
        {
            if (ReferenceEquals(value, null))
                throw new ArgumentNullException(nameof(T));

            var item = new FlashObservationWrapper((FlashObservation)(object)value);

            if (ReferenceEquals(item, null))
                throw new ArgumentNullException(nameof(item));

            try
            {
                var coordinatesWrapper = new CoordinatesWrapper(item.ObservationPoint);

                AddCoordinatesToDatabase(coordinatesWrapper);

                var coordinatesFromDb = GetLastFromDatabase(coordinatesWrapper).Result;

                var coordinatesId = item.CoordinatesId;

                coordinatesId = coordinatesFromDb.Id;

                AddObservationToDatabase(item);
            }
            catch(Exception ex)
            {
                OnError(ex);
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
    }
}
