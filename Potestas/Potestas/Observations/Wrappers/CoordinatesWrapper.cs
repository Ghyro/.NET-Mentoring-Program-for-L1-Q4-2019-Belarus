using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Potestas.Observations.Wrappers
{
    public class CoordinatesWrapper  
    {
        [Key]
        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public List<FlashObservationWrapper> FlashObservations { get; set; }

        public CoordinatesWrapper() { }

        public CoordinatesWrapper(CoordinatesWrapper coordinates)
        {
            X = coordinates.X;
            Y = coordinates.Y;
        }
    }
}
