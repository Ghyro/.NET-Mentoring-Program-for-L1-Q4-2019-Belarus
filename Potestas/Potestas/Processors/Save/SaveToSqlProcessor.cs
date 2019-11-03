using Potestas.Interfaces;
using Potestas.Observations;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Potestas.Processors.Save
{
    public class SaveToSqlProcessor<T> : IEnergyObservationProcessor<T> where T : IEnergyObservation
    {
        private SqlConnection _connectionString;

        public string Description => "SaveToSqlProcessor";

        public SaveToSqlProcessor(string connectionString)
        {
            _connectionString = new SqlConnection(connectionString);
        }

        public void OnCompleted()
        {
            Unsubscribe();
        }

        public void OnError(Exception error)
        {
            _connectionString.Close();
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
                var insertExpession = $"INSERT INTO FlashObservation (Intensity, DurationMs, ObservationTime, EstimatedValue, X, Y) VALUES" +
                    $"(${flash.Intensity}," +
                    $"${flash.DurationMs}," +
                    $"${flash.ObservationTime.ToShortDateString()}," +
                    $"${flash.EstimatedValue}," +
                    $"${flash.ObservationPoint.X.ToString()}," +
                    $"${flash.ObservationPoint.Y.ToString()})";

                var command = new SqlCommand(insertExpession, _connectionString);

                _connectionString.Open();

                command.ExecuteNonQuery();

                OnCompleted();
            }
            catch(Exception ex)
            {
                OnError(ex);
            }           
        }

        public virtual void Unsubscribe()
        {
            _connectionString.Close();
        }
    }
}
