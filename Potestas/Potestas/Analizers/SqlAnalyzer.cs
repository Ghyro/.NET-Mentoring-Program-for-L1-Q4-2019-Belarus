using Potestas.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Potestas.Analizers
{
    public class SqlAnalyzer<T> : IEnergyObservationAnalizer<T> where T : IEnergyObservation
    {
        private SqlConnection _sqlConnection;

        public SqlAnalyzer(string connectionString)
        {
            _sqlConnection = new SqlConnection(connectionString);
        }

        public double GetAverageEnergy()
        {
            var query = @"SELECT AVG(EstimatedValue) FROM FlashObservation";

            var value = 0.0;

            using (_sqlConnection)
            {                
                _sqlConnection.Open();

                var sqlAdapter = new SqlDataAdapter(query, _sqlConnection);

                var dataSet = new DataSet();

                sqlAdapter.Fill(dataSet);

                foreach (DataTable dt in dataSet.Tables)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        value = Convert.ToDouble(row.ItemArray[0]);
                    }
                }
            }

            return value;
        }

        public double GetAverageEnergy(DateTime startFrom, DateTime endBy)
        {
            var query = $"SELECT AVG(EstimatedValue) FROM FlashObservation WHERE ObservationTime > {startFrom} AND ObservationTime < {endBy}";

            var value = 0.0;

            using (_sqlConnection)
            {
                _sqlConnection.Open();

                var sqlAdapter = new SqlDataAdapter(query, _sqlConnection);

                var dataSet = new DataSet();

                sqlAdapter.Fill(dataSet);

                foreach (DataTable dt in dataSet.Tables)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        value = Convert.ToDouble(row.ItemArray[0]);
                    }
                }
            }

            return value;
        }

        public double GetAverageEnergy(Coordinates rectTopLeft, Coordinates rectBottomRight)
        {
            var query = $"SELECT AVG(X), AVG(Y) FROM FlashObservation WHERE X > {rectTopLeft} AND ObservationTime < {rectBottomRight}" +
                $"AND Y < {rectTopLeft} AND Y > {rectBottomRight}";

            var value = 0.0;

            using (_sqlConnection)
            {
                _sqlConnection.Open();

                var sqlAdapter = new SqlDataAdapter(query, _sqlConnection);

                var dataSet = new DataSet();

                sqlAdapter.Fill(dataSet);

                foreach (DataTable dt in dataSet.Tables)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        value = Convert.ToDouble(row.ItemArray[0]);
                    }
                }
            }

            return value;
        }

        public IDictionary<Coordinates, int> GetDistributionByCoordinates()
        {
            //
            throw new NotImplementedException();
        }

        public IDictionary<double, int> GetDistributionByEnergyValue()
        {
            //
            throw new NotImplementedException();
        }

        public IDictionary<DateTime, int> GetDistributionByObservationTime()
        {
            //
            throw new NotImplementedException();
        }

        public double GetMaxEnergy()
        {
            var query = @"SELECT MAX(EstimatedValue) FROM FlashObservation";

            var value = 0.0;

            using (_sqlConnection)
            {
                _sqlConnection.Open();

                var sqlAdapter = new SqlDataAdapter(query, _sqlConnection);

                var dataSet = new DataSet();

                sqlAdapter.Fill(dataSet);

                foreach (DataTable dt in dataSet.Tables)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        value = Convert.ToDouble(row.ItemArray[0]);
                    }
                }
            }

            return value;
        }

        public double GetMaxEnergy(Coordinates coordinates)
        {
            var query = $"SELECT MAX(EstimatedValue) FROM FlashObservation WHERE X = {coordinates.X}";

            var value = 0.0;

            using (_sqlConnection)
            {
                _sqlConnection.Open();

                var sqlAdapter = new SqlDataAdapter(query, _sqlConnection);

                var dataSet = new DataSet();

                sqlAdapter.Fill(dataSet);

                foreach (DataTable dt in dataSet.Tables)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        value = Convert.ToDouble(row.ItemArray[0]);
                    }
                }
            }

            return value;
        }

        public double GetMaxEnergy(DateTime dateTime)
        {
            var query = $"SELECT MAX(EstimatedValue) FROM FlashObservation WHERE ObservationTime = {dateTime}";

            var value = 0.0;

            using (_sqlConnection)
            {
                _sqlConnection.Open();

                var sqlAdapter = new SqlDataAdapter(query, _sqlConnection);

                var dataSet = new DataSet();

                sqlAdapter.Fill(dataSet);

                foreach (DataTable dt in dataSet.Tables)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        value = Convert.ToDouble(row.ItemArray[0]);
                    }
                }
            }

            return value;
        }

        public Coordinates GetMaxEnergyPosition()
        {
            //
            throw new NotImplementedException();
        }

        public DateTime GetMaxEnergyTime()
        {
            var query = $"SELECT TOP 1 EstimatedValue, MAX(ObservationTime) FROM FlashObservation group by EstimatedValue";

            var value = new DateTime();

            using (_sqlConnection)
            {
                _sqlConnection.Open();

                var sqlAdapter = new SqlDataAdapter(query, _sqlConnection);

                var dataSet = new DataSet();

                sqlAdapter.Fill(dataSet);

                foreach (DataTable dt in dataSet.Tables)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        value = (DateTime)row.ItemArray[0];
                    }
                }
            }

            return value;
        }

        public double GetMinEnergy()
        {
            var query = @"SELECT MIN(EstimatedValue) FROM FlashObservation";

            var value = 0.0;

            using (_sqlConnection)
            {
                _sqlConnection.Open();

                var sqlAdapter = new SqlDataAdapter(query, _sqlConnection);

                var dataSet = new DataSet();

                sqlAdapter.Fill(dataSet);

                foreach (DataTable dt in dataSet.Tables)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        value = Convert.ToDouble(row.ItemArray[0]);
                    }
                }
            }

            return value;
        }

        public double GetMinEnergy(Coordinates coordinates)
        {
            var query = $"SELECT MIN(EstimatedValue) FROM FlashObservation WHERE X = {coordinates.X}";

            var value = 0.0;

            using (_sqlConnection)
            {
                _sqlConnection.Open();

                var sqlAdapter = new SqlDataAdapter(query, _sqlConnection);

                var dataSet = new DataSet();

                sqlAdapter.Fill(dataSet);

                foreach (DataTable dt in dataSet.Tables)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        value = Convert.ToDouble(row.ItemArray[0]);
                    }
                }
            }

            return value;
        }

        public double GetMinEnergy(DateTime dateTime)
        {
            var query = $"SELECT MIN(EstimatedValue) FROM FlashObservation WHERE ObservationTime = {dateTime}";

            var value = 0.0;

            using (_sqlConnection)
            {
                _sqlConnection.Open();

                var sqlAdapter = new SqlDataAdapter(query, _sqlConnection);

                var dataSet = new DataSet();

                sqlAdapter.Fill(dataSet);

                foreach (DataTable dt in dataSet.Tables)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        value = Convert.ToDouble(row.ItemArray[0]);
                    }
                }
            }

            return value;
        }

        public Coordinates GetMinEnergyPosition()
        {
            //
            throw new NotImplementedException();
        }

        public DateTime GetMinEnergyTime()
        {
            var query = $"SELECT TOP 1 EstimatedValue, MIN(ObservationTime) FROM FlashObservation group by EstimatedValue";

            var value = new DateTime();

            using (_sqlConnection)
            {
                _sqlConnection.Open();

                var sqlAdapter = new SqlDataAdapter(query, _sqlConnection);

                var dataSet = new DataSet();

                sqlAdapter.Fill(dataSet);

                foreach (DataTable dt in dataSet.Tables)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        value = (DateTime)row.ItemArray[0];
                    }
                }
            }

            return value;
        }
    }
}
