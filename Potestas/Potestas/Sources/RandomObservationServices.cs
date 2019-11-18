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
            var random = new Random();

            return new FlashObservation
            {
                ObservationPoint = new Coordinates
                {
                    X = random.Next(-90, 90),
                    Y = random.Next(0, 180)
                },
                Intensity = random.Next(0, 100),
                DurationMs = random.Next(0, 100),
                ObservationTime = new DateTime(
                random.Next(2000, 2019),
                int.Parse(DateTime.Now.Month.ToString("d2")),
                int.Parse(DateTime.Now.Day.ToString("00")))
            };
        }
    }
}
