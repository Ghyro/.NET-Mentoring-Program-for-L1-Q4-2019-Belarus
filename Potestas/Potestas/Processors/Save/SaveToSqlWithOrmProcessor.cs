using Microsoft.EntityFrameworkCore;
using Potestas.Context;
using Potestas.Interfaces;
using Potestas.Observations;
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

            var item = value as FlashObservation;

            if (ReferenceEquals(item, null))
                throw new ArgumentNullException(nameof(item));

            try
            {
                AddCoordinatesToDatabase(item.ObservationPoint);

                var coordinatesFromDb = GetLastFromDatabase(item.ObservationPoint).Result;

                item.CoordinatesId = coordinatesFromDb.Id;

                AddObservationToDatabase(item);
            }
            catch(Exception ex)
            {
                OnError(ex);
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
    }
}
