using System;
using NUnit.Framework;
using Potestas.Observations;

namespace Potestas.Tests.FlashObservationObject
{
    [TestFixture]
    public class FlashObservationTests
    {
        [Test]
        [TestCase(10, 10, 0.0, 100)]
        [TestCase(20, 10, 0.0, 200)]
        [TestCase(30, 10, 0.0, 300)]
        [TestCase(100, 10, 0.0, 1000)]
        public void Check_EstimatedValue_IntensityMultipleByDurationMs_Value(int duration, int intensity, double estimatedValue, int expected)
        {

            // Arrange
            var flashObservation = new FlashObservation
            {
                ObservationPoint = new Coordinates { X = 1.0, Y = 1.0 },
                DurationMs = 24,
                Intensity = 24,
                ObservationTime = new DateTime(2009, DateTime.Today.Month, DateTime.Today.Day)
            };

            // Act
            flashObservation.DurationMs = duration;
            flashObservation.Intensity = intensity;

            // Assert
            Assert.AreEqual(flashObservation.EstimatedValue, expected);
        }

        [Test]
        [TestCase(2000000001)]
        [TestCase(-1)]
        public void Check_EstimatedValue_IntensityMultipleByDurationMs_ArgumentOutOfRangeException(int parameter)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new FlashObservation
            {
                ObservationPoint = new Coordinates {X = 1.0, Y = 1.0},
                DurationMs = 24,
                Intensity = parameter,
                ObservationTime = new DateTime(2009, DateTime.Today.Month, DateTime.Today.Day)
            });
        }

        [Test]
        public void Check_FlashObservation_EqualObservation_True()
        {
            // Arrange
            var flashObservation1 = new FlashObservation
            {
                ObservationPoint = new Coordinates { X = 1.0, Y = 1.0 },
                DurationMs = 24,
                Intensity = 24,
                ObservationTime = new DateTime(2009, DateTime.Today.Month, DateTime.Today.Day)
            };

            var flashObservation2 = new FlashObservation
            {
                ObservationPoint = new Coordinates { X = 1.0, Y = 1.0 },
                DurationMs = 24,
                Intensity = 24,
                ObservationTime = new DateTime(2009, DateTime.Today.Month, DateTime.Today.Day)
            };

            // Act
            var result = flashObservation1 == flashObservation2;

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Check_FlashObservation_EqualObservation_False()
        {
            // Arrange
            var flashObservation1 = new FlashObservation
            {
                ObservationPoint = new Coordinates { X = 2.0, Y = 2.0 },
                DurationMs = 32,
                Intensity = 32,
                ObservationTime = new DateTime(2009, DateTime.Today.Month, DateTime.Today.Day)
            };

            var flashObservation2 = new FlashObservation
            {
                ObservationPoint = new Coordinates { X = 1.0, Y = 1.0 },
                DurationMs = 24,
                Intensity = 24,
                ObservationTime = new DateTime(2009, DateTime.Today.Month, DateTime.Today.Day)
            };

            // Act
            var result = flashObservation1 == flashObservation2;

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Check_FlashObservationToString_ReturnCorrectStringRepresentOfObservation()
        {
            // Arrange
            var flashObservation = new FlashObservation
            {
                ObservationPoint = new Coordinates { X = 2.0, Y = 2.0 },
                DurationMs = 10,
                Intensity = 10,
                ObservationTime = new DateTime(2019, DateTime.Today.Month, DateTime.Today.Day)
            };

            var month = DateTime.Now.Month.ToString("d2");
            var day = DateTime.Now.Day.ToString("00");
            var year = DateTime.UtcNow.Year.ToString();

            var expectedResult = "ObservationPoint X - Y: 2.0 - 2.0, Intensity: 10, Duration ms: 10," +
                                 $" Observation time: {month}/{day}/{year}, Estimated value: {flashObservation.EstimatedValue}";

            // Act
            var result = flashObservation.ToString();

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Check_FlashObservationConstructor_ReturnCorrectDateTime()
        {
            // Arrange
            var flashObservation = new FlashObservation(DateTime.Today.Date);

            // Assert
            Assert.AreEqual(flashObservation.ObservationTime, DateTime.Today.Date);
        }
    }
}
