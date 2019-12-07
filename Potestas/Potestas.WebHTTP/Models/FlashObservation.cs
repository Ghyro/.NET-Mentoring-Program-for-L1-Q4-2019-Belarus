using Newtonsoft.Json;
using System;

namespace Potestas.WebHTTP.Models
{
    public class FlashObservation
    {
        [JsonProperty("ObservationPoint")]
        public Coordinates ObservationPoint { get; set; }

        [JsonProperty("Intensity")]
        public double Intensity { get; set; }

        [JsonProperty("DurationMs")]
        public int DurationMs { get; set; }

        [JsonProperty("ObservationTime")]
        public DateTime ObservationTime { get; set; }

        [JsonProperty("EstimatedValue")]
        public double EstimatedValue => Intensity * DurationMs;
    }

}
