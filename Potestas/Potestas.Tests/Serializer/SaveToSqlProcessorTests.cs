﻿using NUnit.Framework;
using Potestas.Interfaces;
using Potestas.Observations;
using Potestas.Processors.Save;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Potestas.Tests.Serializer
{
    [TestFixture]
    public class SaveToSqlProcessorTests
    {
        private readonly string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Observations;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        [Test]
        [TestCase(11.2, 14.0, 14.77, 1993)]
        [TestCase(15.99, 2, 0.06, 109)]
        public void SaveToSqlProcessor_SaveToDatabase(double x, double y, double intensity, int duration)
        {
            // Arrange            
            var observations = new List<FlashObservation>();
            var processor = new SaveToSqlProcessor<IEnergyObservation>();
            var observation = new FlashObservation(duration, intensity, new Coordinates(x, y), DateTime.UtcNow);
            var dataRowCount = 0;

            // Act
            processor.OnNext(observation);

            // Assert
            var fetch_query = "SELECT * FROM FlashObservations JOIN Coordinates ON FlashObservations.CoordinatesId = Coordinates.Id";
            using (var _connectionString = new SqlConnection(ConnectionString))
            {
                _connectionString.Open();
                var sqlAdapter = new SqlDataAdapter(fetch_query, _connectionString);

                var dataSet = new DataSet();
                sqlAdapter.Fill(dataSet);

                foreach (DataTable dt in dataSet.Tables)
                {
                    dataRowCount = dt.Rows.Count;

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
                        coordinates.Id = Convert.ToInt32(row.ItemArray[6]);
                        coordinates.X = Convert.ToDouble(row.ItemArray[7]);
                        coordinates.Y = Convert.ToDouble(row.ItemArray[8]);

                        flashObservation.ObservationPoint = coordinates;

                        observations.Add(flashObservation);
                    }
                }
            }

            Assert.AreEqual(observations.Count, dataRowCount);
        }  
    }
}
