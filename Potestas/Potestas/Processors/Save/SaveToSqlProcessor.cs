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

            const string INSERT_COORDINATES_SP = "InsertCoordinates";

            var flashQuery = $"INSERT INTO FlashObservations (Intensity, DurationMs, EstimatedValue, ObservationTime, CoordinatesId)" +
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
                    var commandFlash = new SqlCommand(flashQuery, sqlConnection)
                    {
                        Transaction = sqlTransaction
                    };

                    var commandCoordinates = new SqlCommand(INSERT_COORDINATES_SP, sqlConnection)
                    {
                        CommandType = CommandType.StoredProcedure,
                        Transaction = sqlTransaction
                    };

                    var coordinatesParameters = CreateSqlParametersForStoreProcedury(item);
                    commandCoordinates.Parameters.AddRange(coordinatesParameters);

                    commandCoordinates.ExecuteNonQuery();
                    commandFlash.ExecuteNonQuery();

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

        private static SqlParameter[] CreateSqlParametersForStoreProcedury(IEnergyObservation energyObservation)
        {
            var xParameter = new SqlParameter
            {
                ParameterName = "@X",
                Value = energyObservation.ObservationPoint.X
            };

            var yParameter = new SqlParameter
            {
                ParameterName = "@Y",
                Value = energyObservation.ObservationPoint.Y
            };

            return new[] { xParameter, yParameter };
        }
    }
}
