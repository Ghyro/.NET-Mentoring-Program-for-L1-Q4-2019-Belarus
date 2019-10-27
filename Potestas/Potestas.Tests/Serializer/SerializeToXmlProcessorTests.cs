using NUnit.Framework;
using Potestas.Observations;
using Potestas.Processors.Serializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Potestas.Tests.Serializer
{
    [TestFixture]
    public class SerializeToXmlProcessorTests
    {
        private const string FileName = "test_xml.xml";

        [Test]
        public void XmlSerialize_ThrowNullReferenceException()
        {
            // Arrange
            var fileStream = new FileStream(FileName, FileMode.OpenOrCreate);
            var _xmlSerializer = new SerializeToXMLProcessor<FlashObservation> { Stream = fileStream };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _xmlSerializer.Stream = null);
        }

        [Test]
        [TestCase(11.4, 13.2, 1904.2, 183)]
        [TestCase(19.9, 0, 0.31, 1099)]
        public void XmlSerialize_SerializeObject(double x, double y, double intensity, int duration)
        {
            // Arrange
            var fileStream = new FileStream(FileName, FileMode.OpenOrCreate);
            var _xmlSerializer = new SerializeToXMLProcessor<FlashObservation> { Stream = fileStream };
            var observation = new FlashObservation(duration, intensity, new Coordinates(x, y), DateTime.UtcNow);

            // Act
            _xmlSerializer.OnNext(observation);
            fileStream = new FileStream(FileName, FileMode.Open);

            // Assert
            Assert.AreNotEqual(0, fileStream.Length);
            fileStream.Close();
        }
    }
}
