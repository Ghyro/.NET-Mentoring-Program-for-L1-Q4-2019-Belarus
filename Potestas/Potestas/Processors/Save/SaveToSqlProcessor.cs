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
            Console.WriteLine(error.Message);
        }

        public void OnNext(T value)
        {
            if (ReferenceEquals(value, null))            
                throw new ArgumentNullException(nameof(T));

            var flash = new FlashObservation();

            if (value is FlashObservation)
                flash = (FlashObservation)(object)value;

            try
            {
                using (var sqlConnection = new SqlConnection(ConfigurationManager.AppSettings["ADOConnection"]))
                {
                    var coordinates_query = $"INSERT INTO Coordinates (X, Y) VALUES ({flash.ObservationPoint.X}, {flash.ObservationPoint.Y})";

                    var flash_query = $"INSERT INTO FlashObservations (Intensity, DurationMs, EstimatedValue, ObservationTime, CoordinatesId)" +
                        $" VALUES" +
                        $" ({flash.DurationMs}," +
                        $" {flash.Intensity}," +
                        $" {flash.EstimatedValue}," +
                        $" {flash.ObservationTime.ToShortDateString()}," +
                        $" (SELECT Id FROM Coordinates WHERE X = {flash.ObservationPoint.X} AND Y = {flash.ObservationPoint.Y}))";

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
