using Potestas.Interfaces;
using Potestas.Observations;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Potestas.Processors.Save
{
    public class SaveToSqlProcessor<T> : IEnergyObservationProcessor<T> where T : IEnergyObservation
    {
        public string Description => "SaveToSqlProcessor";

        public void OnCompleted()
        {
            Console.WriteLine("SaveToSqlProcessor has completed");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine();
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
                using (var sqlConnection = new SqlConnection(ConfigurationManager.AppSettings["ADOConnection"]))
                {
                    var coordinates_query = $"INSERT INTO Coordinates (X, Y) VALUES ({item.ObservationPoint.X}, {item.ObservationPoint.Y})";

                    var flash_query = $"INSERT INTO FlashObservations (Intensity, DurationMs, EstimatedValue, ObservationTime, CoordinatesId)" +
                        $" VALUES" +
                        $" ({item.DurationMs}," +
                        $" {item.Intensity}," +
                        $" {item.EstimatedValue}," +
                        $" {item.ObservationTime.ToShortDateString()}," +
                        $" (SELECT Id FROM Coordinates WHERE X = {item.ObservationPoint.X} AND Y = {item.ObservationPoint.Y}))";

                    var command_coordinates = new SqlCommand(coordinates_query, sqlConnection);
                    var command_flash = new SqlCommand(flash_query, sqlConnection);

                    sqlConnection.Open();

                    command_coordinates.ExecuteNonQuery();
                    command_flash.ExecuteNonQuery();

                    OnCompleted();
                }
                
            }
            catch(Exception ex)
            {
                OnError(ex);
            }           
        }
    }
}
