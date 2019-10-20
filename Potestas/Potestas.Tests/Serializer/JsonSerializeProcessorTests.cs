using NUnit.Framework;
using Potestas.Observations;
using Potestas.Processors.Serializers;
using System;
using System.IO;

namespace Potestas.Tests.Serializer
{
    [TestFixture]
    public class JsonSerializeProcessorTests
    {
        private const string FileName = "test_json.json";
        private JsonSerializeProcessor<FlashObservation> _jsonSerialize;

        [Test]
        public void JsonSerialize_ThrowNullReferenceException()
        {
            Assert.Throws<NullReferenceException>(() => _jsonSerialize.Stream = null);
        }

        [Test]
        [TestCase(11.4, 13.2, 1904.2, 183)]
        [TestCase(19.9, 0, 0.31, 1099)]
        public void TestHowOnNextSerializeTheObject(double x, double y, double intensity, int duration)
        {
            // Arrange
            var fileStream = new FileStream(FileName, FileMode.OpenOrCreate);
            _jsonSerialize.Stream = fileStream;
            var observation = new FlashObservation(duration, intensity, new Coordinates(x, y), DateTime.UtcNow);

            // Act
            _jsonSerialize.OnNext(observation);
            fileStream = new FileStream(FileName, FileMode.Open);

            // Assert
            Assert.AreNotEqual(0, fileStream.Length);
            fileStream.Close();
        }
    }
}
