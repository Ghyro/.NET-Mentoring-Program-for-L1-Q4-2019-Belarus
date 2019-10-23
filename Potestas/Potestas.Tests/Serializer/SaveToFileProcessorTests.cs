using NUnit.Framework;
using Potestas.Interfaces;
using Potestas.Observations;
using Potestas.Processors.Save;
using Potestas.Processors.Serializers;
using System;
using System.Configuration;
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
        public void SaveToFileProcessor_SaveToTxtFile_WithoutDecorate(double x, double y, double intensity, int duration)
        {
            // Arrange
            var processor = new SaveToFileProcessor<FlashObservation>(new JsonSerializeProcessor<IEnergyObservation>(),
                ConfigurationManager.AppSettings["processorPath"]);
            var observation = new FlashObservation(duration, intensity, new Coordinates(x, y), DateTime.UtcNow);

            // Act
            processor.FilePath = FileName;
            processor.OnNext(observation);

            // Assert
            Assert.True(File.Exists(FileName));
        }

        [Test]
        [TestCase(11.2, 14.0, 14.77, 1993)]
        [TestCase(15.99, 2, 0.06, 109)]
        public void SaveToFileProcessor_SaveToJsonFile_WithDecorate(double x, double y, double intensity, int duration)
        {
            // Arrange
            var fileStream = new FileStream(FileName, FileMode.OpenOrCreate);
            var serializer = new JsonSerializeProcessor<IEnergyObservation> { Stream = fileStream };
            var processor = new SaveToFileProcessor<IEnergyObservation>(serializer, ConfigurationManager.AppSettings.Get("processorPath"));
            var observation = new FlashObservation(duration, intensity, new Coordinates(x, y), DateTime.UtcNow);

            // Act
            processor.FilePath = JsonFileName;
            processor.OnNext(observation);

            // Assert
            Assert.True(File.Exists(JsonFileName));
            fileStream.Close();
        }
    }
}
