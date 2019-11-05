using Potestas.Interfaces;
using Potestas.Observations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Potestas.Storages
{
    public class SqlStorage<T> : IEnergyObservationStorage<T> where T : IEnergyObservation
    {
        private List<FlashObservation> _observations;

        public SqlStorage()
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
            var fetch_query = "SELECT * FROM FlashObservations JOIN Coordinates ON FlashObservations.CoordinatesId = Coordinates.Id";           

            try
            {
                using (var sqlConnection = new SqlConnection(ConfigurationManager.AppSettings["ADOConnection"]))
                {
                    sqlConnection.Open();
                    var sqlAdapter = new SqlDataAdapter(fetch_query, sqlConnection);

                    var dataSet = new DataSet();
                    sqlAdapter.Fill(dataSet);

                    foreach (DataTable dt in dataSet.Tables)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            var flashObservation = new FlashObservation();
                            var coordinates = new Coordinates();

                            flashObservation.Id = Convert.ToInt32(row.ItemArray[0]);
                            flashObservation.DurationMs = Convert.ToInt32(row.ItemArray[1]);
                            flashObservation.Intensity = Convert.ToDouble(row.ItemArray[2]);
                            flashObservation.EstimatedValue = Convert.ToDouble(row.ItemArray[3]);
                            flashObservation.ObservationTime = (DateTime)row.ItemArray[4];
                            flashObservation.CoordinatesId = Convert.ToInt32(row.ItemArray[5]);
                            coordinates.X = Convert.ToDouble(row.ItemArray[7]);
                            coordinates.Y = Convert.ToDouble(row.ItemArray[8]);

                            flashObservation.ObservationPoint = coordinates;

                            _observations.Add(flashObservation);
                        }
                    }
                }
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }               
        }

        private void InsertToDatabaseTable()
        {
            using (var sqlConnection = new SqlConnection(ConfigurationManager.AppSettings["ADOConnection"]))
            {
                sqlConnection.Open();

                foreach (var flash in _observations)
                {
                    var coordinates_query = $"INSERT INTO Coordinates (X, Y) VALUES ({flash.ObservationPoint.X}, {flash.ObservationPoint.Y})";

                    var flash_query = $"INSERT INTO FlashObservations (Intensity, DurationMs, ObservationTime, EstimatedValue, CoordinatesId)" +
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
                }                
            }
        }

        private void ClearDatabaseTable()
        {
            var deleteCoordinates_query = "DELETE FROM Coordinates";
            var deleteFlash_query = "DELETE FROM FlashObservations";

            using (var sqlConnection = new SqlConnection(ConfigurationManager.AppSettings["ADOConnection"]))
            {
                sqlConnection.Open();

                var command_1 = new SqlCommand(deleteFlash_query, sqlConnection);       
                var command_1 = new SqlCommand(deleteCoordinates_query, sqlConnection); 

                command_1.ExecuteNonQuery();
                command_2.ExecuteNonQuery();
            }
        }

        #endregion
    }
}
