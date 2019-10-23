using System;
using NUnit.Framework;

namespace Potestas.Tests.CoordinatesObject
{
    [TestFixture]
    public class CoordinatesTests
    {
        [Test]
        public void Coordinates_ShouldCreateNewObject()
        {
            // Arrange
            var coordinates = new Coordinates(50.0, 30.0);

            // Assert
            Assert.IsNotNull(coordinates);
            Assert.AreEqual(coordinates.X, 50.0);
            Assert.AreEqual(coordinates.Y, 30.0);
        }

        [Test]
        [TestCase(90.0, 90.0)]
        [TestCase(60.0, 60.0)]
        public void Coordinates_ShouldCheckForEquals(double x, double y)
        {
            // Arrange
            var coordinates1 = new Coordinates(x, y);
            var coordinates2 = new Coordinates(x, y);

            // Act
            var expected = coordinates1 == coordinates2;

            // Assert
            Assert.IsTrue(expected);
        }

        [Test]
        public void Coordinates_ShouldCheckForEquals_False()
        {
            // Arrange
            var coordinates1 = new Coordinates(90.0, 90.0);
            var coordinates2 = new Coordinates(60.0, 90.0);

            // Act
            var expected = coordinates1 == coordinates2;

            // Assert
            Assert.IsFalse(expected);
        }

        [Test]
        [TestCase(5.0, 5.0, 10.0, 10.0)]
        [TestCase(10.0, 5.0, 20.0, 10.0)]
        public void Coordinates_ShouldSumCoordinates(double x, double y, double xResult, double yResult)
        {
            // Arrange
            var coordinates1 = new Coordinates(x, y);
            var coordinates2 = new Coordinates(x, y);

            var expectedCoordinates = new Coordinates(xResult, yResult);

            // Act
            var expected = coordinates1 + coordinates2;

            // Assert
            Assert.AreEqual(expected, expectedCoordinates);
        }

        [Test]
        [TestCase(20.0, 5.0, 10.0, 2.0)]
        [TestCase(15.0, 5.0, 5.0, 2.0)]
        public void Coordinates_ShouldSubtractionCoordinates(double x, double y, double xResult, double yResult)
        {
            // Arrange
            var coordinates1 = new Coordinates(x, y);
            var coordinates2 = new Coordinates(10.0, 3.0);

            var expectedCoordinates = new Coordinates(xResult, yResult);

            // Act
            var expected = coordinates1 - coordinates2;

            // Assert
            Assert.AreEqual(expected, expectedCoordinates);
        }

        [Test]
        [TestCase(-91.0, 180.0)]
        [TestCase(-90.0, 181.0)]
        [TestCase(-91.0, 181.0)]
        public void Coordinates_ShouldCheckValidRange_ArgumentOutOfRangeException(double x, double y)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Coordinates { X = x,Y = y });
        }

        [Test]
        public void Coordinates_ShouldReturnCorrectToString()
        {
            // Arrange
            var coordinates1 = new Coordinates(10.0, 3.0);
            var expectedString = $"X: {coordinates1.X}, Y: {coordinates1.Y}";

            // Act
            var resultString = coordinates1.ToString();

            // Assert
            Assert.AreEqual(expectedString, resultString);
        }
    }
}
