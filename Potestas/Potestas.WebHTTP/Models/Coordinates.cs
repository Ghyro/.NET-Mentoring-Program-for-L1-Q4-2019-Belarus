using Newtonsoft.Json;

namespace Potestas.WebHTTP.Models
{
    public class Coordinates
    {
        [JsonProperty("X")]
        public double X { get; set; }

        [JsonProperty("Y")]
        public double Y { get; set; }
    }

}
