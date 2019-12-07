//using NUnit.Framework;
//using Potestas.Context;
//using Potestas.Interfaces;
//using Potestas.Observations;
//using Potestas.Processors.Save;
//using System;
//using System.Linq;

//namespace Potestas.Tests.Serializer
//{
//    [TestFixture]
//    public class SaveToSqlWithOrmProcessorTests
//    {
//        private ObservationContext DbContext = new ObservationContext();

//        [Test]
//        [TestCase(11.2, 14.0, 14.77, 1993)]
//        public void SaveToSqlWithOrmProcessor_SaveToDatabase(double x, double y, double intensity, int duration)
//        {
//            // Arrange            
//            var processor = new SaveToSqlWithOrmProcessor<IEnergyObservation>(DbContext);
//            var observation = new FlashObservation(duration, intensity, new Coordinates(x, y), DateTime.UtcNow);

//            // Act
//            processor.OnNext(observation);

//            // Assert
//            using (DbContext)
//            {
//                var flashObservation = DbContext.FlashObservationWrapper.FirstOrDefault(s => s.DurationMs == observation.DurationMs
//                && s.Intensity == observation.Intensity);
//                Assert.IsNotNull(flashObservation);

//                var coordinates = DbContext.CoordinatesWrapper.FirstOrDefault(v => v.Id == flashObservation.Id);
//                Assert.IsNotNull(coordinates);

//                var flshWrapPoint = flashObservation.ObservationPoint;

//                flshWrapPoint.Id = coordinates.Id;
//                flshWrapPoint.X = coordinates.X;
//                flshWrapPoint.Y = coordinates.Y;

//                Assert.AreEqual(flashObservation.ObservationPoint.X, observation.ObservationPoint.X);
//                Assert.AreEqual(flashObservation.ObservationPoint.Y, observation.ObservationPoint.Y);
//            }
//        }
//    }
//}
            