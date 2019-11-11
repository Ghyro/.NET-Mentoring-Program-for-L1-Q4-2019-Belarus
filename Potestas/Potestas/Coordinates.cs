using Potestas.Observations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Potestas
{
    /* TASK. Implement this structure: 
     * 1. Implement custom constructor
     * 2. The valid range for X is [-90; 90], for Y [0; 180]
     * 3. Take into account boxing and unboxing issues
     * 4. Implement + and - operators for this structure.
     * 5. Implement a way to represent coordinates in string.
     * 6. Coordinates are equal each other when each dimension values are equal with thousand precision
     * 7. Implement == and != operators for this structure.
     * 8. 
     */

    [DataContract]
    [Serializable]
    public class Coordinates
    {
        [DataMember]
        [Key]
        public int Id { get; set; }
        public List<FlashObservation> FlashObservations { get; set; }
        private double _x;
        private double _y;

        public Coordinates() { }

        public Coordinates(double x, double y)
        {
            X = x;
            Y = y; 
        }        

        [DataMember]
        public double X
        {
            get => _x;
            set
            {
                if (value < -90 || value > 90)
                    throw new ArgumentOutOfRangeException(nameof(value));
                _x = value;
            }
        }

        [DataMember]
        public double Y
        {
            get => _y;
            set
            {
                if (value < 0 || value > 180)
                    throw new ArgumentOutOfRangeException(nameof(value));
                _y = value;
            }
        }

        public static bool operator ==(Coordinates coordinates1, Coordinates coordinates2)
        {
            return coordinates1.Equals(coordinates2);
        }
        
        public static bool operator !=(Coordinates coordinates1, Coordinates coordinates2)
        {
            return !coordinates1.Equals(coordinates2);
        }

        public static Coordinates operator +(Coordinates coordinates1, Coordinates coordinates2)
        {
            return new Coordinates(coordinates1.X + coordinates2.X, coordinates1.Y + coordinates2.Y);
        }

        public static Coordinates operator -(Coordinates coordinates1, Coordinates coordinates2)
        {
            return new Coordinates(coordinates1.X - coordinates2.X, coordinates1.Y - coordinates2.Y);
        }

        public override bool Equals(object obj)
        {
            if (obj is Coordinates coordinates)
                return Math.Round(coordinates.X, 3) == Math.Round(this.X, 3) && Math.Round(coordinates.Y, 3) == Math.Round(this.Y, 3);
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_x.GetHashCode() * 397) ^ _y.GetHashCode();
            }
        }

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}";
        }
    }
}
