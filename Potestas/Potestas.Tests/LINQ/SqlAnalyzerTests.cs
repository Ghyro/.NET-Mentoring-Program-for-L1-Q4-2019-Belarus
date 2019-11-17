using NUnit.Framework;
using Potestas.Analyzers;
using Potestas.Interfaces;

namespace Potestas.Tests.LINQ
{
    [TestFixture]
    public class SqlAnalizerTests
    {
        private readonly string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ObservationCenter;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        [TestCase(4401)]
        public void GetAverageEnergyTest_ReturnAverageEnergy(double expectedResult)
        {
            // Arange
            var sqlAnalizer = new SqlAnalyzer<IEnergyObservation>(_connectionString);

            // Act
            var result = sqlAnalizer.GetAverageEnergy();

            // Assert
            Assert.AreEqual(expectedResult, result, 1);
        }

        [Test]
        [TestCase(29437)]
        public void GetMaxEnergyTest_ReturnAverageEnergy(double expectedResult)
        {
            // Arange
            var sqlAnalizer = new SqlAnalyzer<IEnergyObservation>(_connectionString);

            // Act
            var result = sqlAnalizer.GetMaxEnergy();

            // Assert
            Assert.AreEqual(expectedResult, result, 1);
        }
    }
}
