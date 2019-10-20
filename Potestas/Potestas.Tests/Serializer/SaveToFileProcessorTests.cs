using NUnit.Framework;
using Potestas.Observations;
using Potestas.Processors.Save;
using Potestas.Processors.Serializers;
using System;
using System.IO;

namespace Potestas.Tests.Serializer
{
    [TestFixture]
    public class SaveToFileProcessorTests
    {
        private const string JsonFileName = @"test_json_SaveToFileProcessor.json";
        private const string FileName = @"test_txt_SaveToFileProcessor.txt";

        [Test]
        [TestCase(11.2, 14.0, 14.77, 1993)]
        [TestCase(15.99, 2, 0.06, 109)]
        public void SaveWithOwnLogicTest(double x, double y, double intensity, int duration)
        {
            // Arrange
            var processor = new SaveToFileProcessor<FlashObservation>(null);
            var observation = new FlashObservation(duration, intensity, new Coordinates(x, y), DateTime.UtcNow);

            // Act
            processor.FileName = FileName;
            processor.OnNext(observation);

            // Assert
            Assert.True(File.Exists(FileName));
        }

        [Test]
        [TestCase(11.2, 14.0, 14.77, 1993)]
        [TestCase(15.99, 2, 0.06, 109)]
        public void SaveWithDecoratedComponentLogicTest(double x, double y, double intensity, int duration)
        {
            // Arrange
            var serializer = new JsonSerializeProcessor<FlashObservation>();
            var processor = new SaveToFileProcessor<FlashObservation>(serializer);
            var observation = new FlashObservation(duration, intensity, new Coordinates(x, y), DateTime.UtcNow);

            // Act
            processor.FileName = JsonFileName;
            processor.OnNext(observation);

            // Assert
            Assert.True(File.Exists(JsonFileName));
        }
    }
}
