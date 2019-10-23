using System;
using System.Collections.Generic;
using NUnit.Framework;
using Potestas.Comparators;
using Potestas.Interfaces;
using Potestas.Observations;

namespace Potestas.Tests.Comparers
{
    [TestFixture]
    public class FlashEqualityComparerTests
    {
        private static readonly EnergyObservationEqualityComparer EqualityComparer = new EnergyObservationEqualityComparer();
        private static readonly Dictionary<IEnergyObservation, string> FlashObservarions = new Dictionary<IEnergyObservation, string>(EqualityComparer);

        private static readonly IEnergyObservation FlashObservation1 = new FlashObservation
        {
            ObservationPoint = new Coordinates { X = 3.0, Y = 0.5 },
            ObservationTime = new DateTime(2009, DateTime.Today.Month, DateTime.Today.Day)
        };

        private static readonly IEnergyObservation FlashObservation2 = new FlashObservation
        {
            ObservationPoint = new Coordinates { X = 1.5, Y = 0.3 },
            ObservationTime = new DateTime(2008, DateTime.Today.Month, DateTime.Today.Day)
        };

        [Test]
        public void FlashEqualityComparer_AddFlashObservations_DifferentKeys_PositiveTest()
        {
            // Arrange
            var expected = new Dictionary<IEnergyObservation, string>
            {
                {FlashObservation1, "theFirstObservation"},
                {FlashObservation2, "theSecondObservation"}
            };

            // Act
            AddObservation(FlashObservarions, FlashObservation1, "theFirstObservation");
            AddObservation(FlashObservarions, FlashObservation2, "theSecondObservation");

            // Assert
            Assert.AreEqual(expected, FlashObservarions, "Two collections is not equal");
        }

        [Test]
        public void FlashEqualityComparer_AddFlashObservations_DuplicateKeys_ArgumentException()
        {
            // Act
            AddObservation(FlashObservarions, FlashObservation1, "theFirstObservation");
            var exception = AddObservation(FlashObservarions, FlashObservation1, "theSecondObservation");

            // Assert
            Assert.IsTrue(exception);
        }

        #region private

        private static bool AddObservation(IDictionary<IEnergyObservation, string> observations,
            IEnergyObservation flashObservation, string description)
        {
            try
            {
                observations.Add(flashObservation, description);

                return false;
            }
            catch (ArgumentException)
            {
                return true;
            }
        }

        #endregion
    }
}
