using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Potestas.Analizers;
using Potestas.Interfaces;
using Potestas.Observations;

namespace Potestas.Tests.LINQ
{
    [TestFixture]
    public class LINQAnalizerTests
    {
        [Test]
        [TestCase(74.8)]
        public void GetAverageEnergyTest_ReturnAverageEnergy(double expectedResult)
        {
            // Arange
            var linqAnalizer = new LINQAnalizer(MockStorage().Object);

            // Act
            var result = linqAnalizer.GetAverageEnergy();

            // Assert
            Assert.AreEqual(expectedResult, result, 1);
        }

        [Test]
        [TestCase(242)]
        public void GetMaxEnergyTest_ReturnAverageEnergy(double expectedResult)
        {
            // Arange
            var linqAnalizer = new LINQAnalizer(MockStorage().Object);

            // Act
            var result = linqAnalizer.GetMaxEnergy();

            // Assert
            Assert.AreEqual(expectedResult, result, 1);
        }


        [Test]
        [TestCase(2019, 10, 15, 242)]
        [TestCase(2019, 09, 01, 2)]
        [TestCase(2018, 08, 15, 126)]
        public void GetMaxEnergyTest_PassDateTimeRange_ReturnAverageEnergy(int yearStart, int monthStart, int dayStart, double expectedResult)
        {
            // Arange
            var dateObservation = new DateTime(yearStart, monthStart, dayStart);
            var linqAnalizer = new LINQAnalizer(MockStorage().Object);

            // Act
            var result = linqAnalizer.GetMaxEnergy(dateObservation);

            // Assert
            Assert.AreEqual(expectedResult, result, 1);
        }

        #region private
        private Mock<IEnergyObservationStorage<IEnergyObservation>> MockStorage()
        {
            var energyObservations = new List<IEnergyObservation>
            {
                new FlashObservation(1, 2, new Coordinates(11, 22), new DateTime(2019, 10, 15)),
                new FlashObservation(11, 22, new Coordinates(11, 22), new DateTime(2019, 10, 15)),
                new FlashObservation(1, 2, new Coordinates(1, 2), new DateTime(2019, 09, 01)),
                new FlashObservation(1, 2, new Coordinates(14, 14), new DateTime(2018, 10, 15)),
                new FlashObservation(9, 14, new Coordinates(14, 14), new DateTime(2018, 08, 15))
            };

            var storageMock = new Mock<IEnergyObservationStorage<IEnergyObservation>>();

            storageMock.Setup(x => x.GetEnumerator()).Returns(() => energyObservations.GetEnumerator());

            return storageMock;
        }
        #endregion
    }
}
