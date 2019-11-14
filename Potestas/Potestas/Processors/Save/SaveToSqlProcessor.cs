using Potestas.Interfaces;
using Potestas.Observations;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

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

            var item = (FlashObservation)(object)value;

            if (ReferenceEquals(item, null))
                throw new ArgumentNullException(nameof(item));

            var insertCoordinates_storeProcedure = "InsertCoordinates";

            var flash_query = $"INSERT INTO FlashObservations (Intensity, DurationMs, EstimatedValue, ObservationTime, CoordinatesId)" +
                        $" VALUES" +
                        $" ({item.DurationMs}," +
                        $" {item.Intensity}," +
                        $" {item.EstimatedValue}," +
                        $" {item.ObservationTime.ToShortDateString()}," +
                        $" (SELECT Id FROM Coordinates WHERE X = {item.ObservationPoint.X} AND Y = {item.ObservationPoint.Y}))";

            using (var sqlConnection = new SqlConnection(ConfigurationManager.AppSettings["ADOConnection"]))
            {
                sqlConnection.Open();
                var sqlTransaction = sqlConnection.BeginTransaction();

                try
                {    
                    var command_flash = new SqlCommand(flash_query, sqlConnection)
                    {
                        Transaction = sqlTransaction
                    };

                    var command_coordinates = new SqlCommand(insertCoordinates_storeProcedure, sqlConnection)
                    {
                        CommandType = CommandType.StoredProcedure,
                        Transaction = sqlTransaction
                    };

                    var coordinatesParameters = CreateSqlParametersForStoreProcedury(item);
                    command_coordinates.Parameters.AddRange(coordinatesParameters);

                    command_coordinates.ExecuteNonQuery();
                    command_flash.ExecuteNonQuery();

                    sqlTransaction.Commit();

                    OnCompleted();
                }
                catch (Exception ex)
                {
                    sqlTransaction.Rollback();
                    OnError(ex);
                }
            }       
        }

        private SqlParameter[] CreateSqlParametersForStoreProcedury(IEnergyObservation energyObservation)
        {
            var x_Parameter = new SqlParameter
            {
                ParameterName = "@X",
                Value = energyObservation.ObservationPoint.X
            };

            var y_Parameter = new SqlParameter
            {
                ParameterName = "@Y",
                Value = energyObservation.ObservationPoint.Y
            };

            return new SqlParameter[] { x_Parameter, y_Parameter };
        }
    }
}
