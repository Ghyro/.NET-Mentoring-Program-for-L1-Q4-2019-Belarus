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
        private SqlConnection _connectionString;
        private List<FlashObservation> _observations;

        public SqlStorage(string connectionString)
        {
            _observations = new List<FlashObservation>();
            _connectionString = new SqlConnection(connectionString);
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
            var fetchExpression = "SELECT * FROM FlashObservation";            

            using (_connectionString)
            {
                _connectionString.Open();
                var sqlAdapter = new SqlDataAdapter(fetchExpression, _connectionString);

                var dataSet = new DataSet();
                sqlAdapter.Fill(dataSet);

                foreach (DataTable dt in dataSet.Tables)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var flashObservation = new FlashObservation();
                        var coordinates = new Coordinates();

                        flashObservation.Id = int.Parse(row.ItemArray[0].ToString());
                        flashObservation.Intensity = Convert.ToDouble(row.ItemArray[1]);
                        flashObservation.DurationMs = Convert.ToInt32(row.ItemArray[2]);
                        flashObservation.ObservationTime = (DateTime)(row.ItemArray[3]);
                        flashObservation.EstimatedValue = Convert.ToDouble(row.ItemArray[4]);
                        coordinates.X = Convert.ToDouble(row.ItemArray[5]);
                        coordinates.Y = Convert.ToDouble(row.ItemArray[6]);

                        flashObservation.ObservationPoint = coordinates;

                        _observations.Add(flashObservation);
                    }
                }
            }
        }

        private void InsertToDatabaseTable()
        {
            using (_connectionString)
            {
                foreach(var flash in _observations)
                {
                    var insertExpession = $"INSERT INTO FlashObservation (Intensity, DurationMs, ObservationTime, EstimatedValue, X, Y) VALUES" +
                    $"(${flash.Intensity.ToString()}," +
                    $"${flash.DurationMs.ToString()}," +
                    $"${flash.ObservationTime.ToShortDateString()}," +
                    $"${flash.EstimatedValue.ToString()}," +
                    $"${flash.ObservationPoint.X.ToString()}," +
                    $"${flash.ObservationPoint.Y.ToString()})";

                    var command = new SqlCommand(insertExpession, _connectionString);

                    _connectionString.Open();

                    command.ExecuteNonQuery();
                }                
            }
        }

        private void ClearDatabaseTable()
        {
            var deleteExpression = "DELETE FROM FlashObservation";

            using (_connectionString)
            {
                var command = new SqlCommand(deleteExpression, _connectionString);

                _connectionString.Open();

                command.ExecuteNonQuery();
            }
        }

        #endregion
    }
}
