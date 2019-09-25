using System;
using System.Collections.Generic;
using NUnit.Framework;
using Potestas.Comparators;
using Potestas.Observations;

namespace Potestas.Tests.Comparers
{
    [TestFixture]
    public class FlashComparerTests
    { 
        [Test]
        public void FlashComparer_ComparesBySort()
        {
            // Arrange
            var flashObservation1 = new FlashObservation
            {
                ObservationPoint = new Coordinates { X = 1.0, Y = 1.0 },
                EstimatedValue = 2.0,
                ObservationTime = new DateTime(2009, DateTime.Today.Month, DateTime.Today.Day)
            };

            var flashObservation2 = new FlashObservation
            {
                ObservationPoint = new Coordinates { X = 1.0, Y = 1.0 },
                EstimatedValue = 1.0,
                ObservationTime = new DateTime(2008, DateTime.Today.Month, DateTime.Today.Day)
            };

            var flashObservation3 = new FlashObservation
            {
                ObservationPoint = new Coordinates { X = 3.0, Y = 3.0 },
                EstimatedValue = 3.0,
                ObservationTime = new DateTime(2010, DateTime.Today.Month, DateTime.Today.Day)
            };

            var observations = new List<IEnergyObservation> { flashObservation1, flashObservation2, flashObservation3 };
            var expectedResult =  new List<IEnergyObservation> { flashObservation2, flashObservation1, flashObservation3 };

            // Act
            observations.Sort(new FlashComparer());

            // Assert
            Assert.AreEqual(expectedResult, observations, "Two collections is not equal");
        }

        [Test]
        public void FlashComparer_ComparesTwoCollections()
        {
            // Arrange            
            var flashObservation1 = new FlashObservation
            {
                ObservationPoint = new Coordinates { X = 1.0, Y = 1.0 },
                EstimatedValue = 2.0,
                ObservationTime = new DateTime(2009, DateTime.Today.Month, DateTime.Today.Day)
            };

            var flashObservation2 = new FlashObservation
            {
                ObservationPoint = new Coordinates { X = 1.0, Y = 1.0 },
                EstimatedValue = 2.0,
                ObservationTime = new DateTime(2009, DateTime.Today.Month, DateTime.Today.Day)
            };

            var result = new List<IEnergyObservation>();
            var expectedResult = new List<IEnergyObservation>();

            // Act
            result.AddRange(new List<IEnergyObservation>{ flashObservation1, flashObservation2 });
            expectedResult.AddRange(new List<IEnergyObservation> { flashObservation2, flashObservation1 });

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void FlashComparer_ComparesTwoCollections_CatchAssertException()
        {
            // Arrange            
            var flashObservation1 = new FlashObservation
            {
                ObservationPoint = new Coordinates { X = 1.0, Y = 1.0 },
                EstimatedValue = 2.0,
                ObservationTime = new DateTime(2009, DateTime.Today.Month, DateTime.Today.Day)
            };

            var flashObservation2 = new FlashObservation
            {
                ObservationPoint = new Coordinates { X = 1.0, Y = 1.0 },
                EstimatedValue = 10.0,
                ObservationTime = new DateTime(2009, DateTime.Today.Month, DateTime.Today.Day)
            };

            var result = new List<IEnergyObservation>();
            var expectedResult = new List<IEnergyObservation>();

            // Act
            result.AddRange(new List<IEnergyObservation> { flashObservation1, flashObservation2 });
            expectedResult.AddRange(new List<IEnergyObservation> { flashObservation2, flashObservation1 });

            // Assert
            Assert.Throws<AssertionException>(() => Assert.That(result, Is.EqualTo(expectedResult)));
        }
    }
}
