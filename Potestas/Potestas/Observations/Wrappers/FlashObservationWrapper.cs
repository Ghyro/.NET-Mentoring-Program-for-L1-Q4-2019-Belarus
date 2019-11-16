using System;
using System.ComponentModel.DataAnnotations;

namespace Potestas.Observations.Wrappers
{
    public class FlashObservationWrapper
    {
        [Key]
        public int Id { get; set; }
        public double Intensity { get; set; }
        public int DurationMs { get; set; }
        public DateTime ObservationTime { get; set; }
        public double EstimatedValue { get; set; }
        public int CoordinatesId { get; set; }
        public CoordinatesWrapper ObservationPoint { get; set; }

        public FlashObservationWrapper() { }

        public FlashObservationWrapper(FlashObservation observation)
        {
            Intensity = observation.Intensity;
            DurationMs = observation.DurationMs;
            ObservationTime = observation.ObservationTime;
            EstimatedValue = observation.EstimatedValue;
            ObservationPoint = CreateCoordinatesWrapper(observation);
        }

        private static CoordinatesWrapper CreateCoordinatesWrapper(FlashObservation observation)
        {
            return new CoordinatesWrapper
            {
                X = observation.ObservationPoint.X,
                Y = observation.ObservationPoint.Y
            };
        }
    }
}
