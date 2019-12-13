using System;

namespace Potestas.Web.Models
{
    public class FlashObservationViewModel
    {
        public int Id { get; set; }

        public CoordinatesViewModel ObservationPoint { get; set; }

        public double EstimatedValue { get; set; }

        public DateTime ObservationTime { get; set; }
    }
}
