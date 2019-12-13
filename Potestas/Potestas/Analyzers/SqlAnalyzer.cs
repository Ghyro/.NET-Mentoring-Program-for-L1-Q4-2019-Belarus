using Potestas.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Potestas.Analyzers
{
    public class SqlAnalyzer<T> : IEnergyObservationAnalyzer<T> where T : IEnergyObservation
    {
        private SqlConnection _sqlConnection;

        public SqlAnalyzer(string connectionString)
        {
            _sqlConnection = new SqlConnection(connectionString);
        }

        public double GetAverageEnergy()
        {
            var query = @"SELECT AVG(EstimatedValue) FROM FlashObservations";

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
            var query = $"SELECT AVG(EstimatedValue) FROM FlashObservations WHERE ObservationTime > {startFrom} AND ObservationTime < {endBy}";

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
            var query = $"SELECT AVG(X), AVG(Y) FROM FlashObservations WHERE X > {rectTopLeft} AND ObservationTime < {rectBottomRight}" +
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
            var query = @"SELECT Coordinates.Id, Coordinates.X, Coordinates.Y, COUNT(FlashObservations.Id) FROM FlashObservations
                          JOIN Coordinates ON Coordinates.Id = FlashObservations.CoordinatesId
                          GROUP BY Coordinates.Id, Coordinates.X,Coordinates.Y";

            var result = new Dictionary<Coordinates, int>();

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
                        var coordinates = new Coordinates
                        {
                            X = Convert.ToDouble(row.ItemArray[1]),
                            Y = Convert.ToDouble(row.ItemArray[2])
                        };

                        result.Add(coordinates, Convert.ToInt32(row.ItemArray[3]));
                    }
                }
            }

            return result;
        }

        public IDictionary<double, int> GetDistributionByEnergyValue()
        {
            var query = @"SELECT EstimatedValue, COUNT(Id) FROM FlashObservations GROUP BY EstimatedValue";

            var result = new Dictionary<double, int>();

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
                        result.Add(Convert.ToDouble(row.ItemArray[0]), Convert.ToInt32(row.ItemArray[1]));
                    }
                }
            }

            return result;
        }

        public IDictionary<DateTime, int> GetDistributionByObservationTime()
        {
            var query = @"SELECT ObservationTime, COUNT(Id) FROM FlashObservations GROUP BY ObservationTime";

            var result = new Dictionary<DateTime, int>();

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
                        result.Add((DateTime)row.ItemArray[0], Convert.ToInt32(row.ItemArray[1]));
                    }
                }
            }

            return result;
        }

        public double GetMaxEnergy()
        {
            var query = @"SELECT MAX(EstimatedValue) FROM FlashObservations";

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
            var query = $"SELECT MAX(EstimatedValue) FROM FlashObservations as F JOIN Coordinates as C ON F.CoordinatesId = C.Id" +
                $"WHERE C.X = {coordinates.X} AND C.Y = {coordinates.Y}";

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
            var query = $"SELECT MAX(EstimatedValue) FROM FlashObservations as F JOIN Coordinates as C ON F.CoordinatesId = C.Id" +
                $"WHERE F.ObservationTime = {dateTime.ToShortDateString()}";

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
            var query = $"SELECT TOP 1 FROM FlashObservations as F JOIN Coordinates as C ON F.CoordinatesId = C.Id" +
                $"WHERE F.EstimatedValue - (SELECT MAX(EstimatedValue) FROM FlashObservations) < 0.001)";

            var value = new Coordinates();

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
                        value.X = Convert.ToDouble(row.ItemArray[1]);
                        value.Y = Convert.ToDouble(row.ItemArray[2]);
                    }
                }
            }

            return value;
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
            var query = @"SELECT MIN(EstimatedValue) FROM FlashObservations";

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
            var query = $"SELECT MIN(EstimatedValue) FROM FlashObservations as F JOIN Coordinates as C ON F.CoordinatesId = C.Id" +
                $"WHERE C.X = {coordinates.X} AND C.Y = {coordinates.Y}";

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
            var query = $"SELECT MIN(EstimatedValue) FROM FlashObservations as F JOIN Coordinates as C ON F.CoordinatesId = C.Id" +
                $"WHERE F.ObservationTime = {dateTime.ToShortDateString()}";

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
            var query = $"SELECT TOP 1 FROM FlashObservations as F JOIN Coordinates as C ON F.CoordinatesId = C.Id" +
                 $"WHERE F.EstimatedValue - (SELECT MIN(EstimatedValue) FROM FlashObservations) < 0.001)";

            var value = new Coordinates();

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
                        value.X = Convert.ToDouble(row.ItemArray[1]);
                        value.Y = Convert.ToDouble(row.ItemArray[2]);
                    }
                }
            }

            return value;
        }

        public DateTime GetMinEnergyTime()
        {
            var query = $"SELECT TOP 1 EstimatedValue, MIN(ObservationTime) FROM FlashObservations GROUP BY EstimatedValue";

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
