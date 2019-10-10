using Potestas.Observations;
using System;

namespace Potestas.Sources
{
    public static class RandomObservationServices
    {
        private static Coordinates _observationPoint;
        private static double _intensity;
        private static int _durationMs;
        private static DateTime _observationTime;

        public static FlashObservation CreateRandomObservation()
        {
            SetRandomValues();

            return new FlashObservation
            {
                ObservationPoint = _observationPoint,
                DurationMs = _durationMs,
                Intensity = _intensity,
                ObservationTime = _observationTime
            };
        }

        private static void SetRandomValues()
        {
            var random = new Random();

            _observationPoint = new Coordinates
            {
                X = random.Next(-90, 90),
                Y = random.Next(0, 180)
            };

            _intensity = random.Next(0, 2000000000);
            _durationMs = random.Next(0, int.MaxValue);

            _observationTime = new DateTime(
                random.Next(2000, 2019),
                int.Parse(DateTime.Now.Month.ToString("d2")),
                int.Parse(DateTime.Now.Day.ToString("00")));
        }
    }
}
