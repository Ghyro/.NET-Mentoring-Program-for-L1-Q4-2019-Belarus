﻿using System;
using System.Runtime.Serialization;
using Potestas.Interfaces;

namespace Potestas.Observations
{
    /* TASK: Implement this structure by following requirements:
    * 1. EstimatedValue is the intensity multiple by duration
    * 2. Observations are equal if they made at the same time, 
    * the same observation point and EstimatedValue 
    * is the same by decimal presicion
    * 3. Implement custom constructors with ability to set ObservationTime by moment of creation or from constructor parameter.
    * 4. Implement == and != operators for the structure.
    * 6. Negative Intensity is a sign of invalid observation. Figure out how to process such errors. Remember you are writing a library.
    * 7. Intensity more than 2 000 000 000 is imposible and could be a sign of the invalid observation.
    * 8. Implement nice string representation of this observation.
    * QUESTIONS: 
    * How implementation of interface impacts boxing and unboxing operation for the structure?
    * Why overriding of Equals method is not enough?
    * What kind of pollymorhism does this struct contain?
    * Why immutable structure is used here?
    * TESTS: Cover this structure with unit tests
    */

    [DataContract]
    [Serializable]
    public struct FlashObservation : IEnergyObservation
    {
        private const int MAX_INTENSITY = 2000000000;
        private double _intensity;
        private int _durationMs;
        private DateTime _observationTime;
        private double _estimatedValue;
        private int _id;
        private int _coordinatesId;

        public FlashObservation(DateTime observationTime) : this()
        {
            ObservationTime = observationTime;
        }

        public FlashObservation(int durationMs, double intensity, Coordinates observationPoint, DateTime observationTime) : this()
        {
            DurationMs = durationMs;
            Intensity = intensity;
            ObservationPoint = observationPoint;
            ObservationTime = observationTime;
        }

        [DataMember]
        
        public int Id { get; set; }

        [DataMember]
        public int DurationMs
        {
            get => _durationMs;
            set => _durationMs = value;
        }

        [DataMember]
        public double Intensity
        {
            get => _intensity;
            set
            {
                if (value > MAX_INTENSITY || value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));
                _intensity = value;
            }
        }

        [DataMember]
        public double EstimatedValue
        {
            get
            {
                checked
                {
                    return _intensity * _durationMs;
                }
            }
            set => _estimatedValue = value;
        }

        [DataMember]
        public DateTime ObservationTime
        {
            get => _observationTime;
            set => _observationTime = value;
        }

        [DataMember]
        public int CoordinatesId
        {
            get => _coordinatesId;
            set
            {
                if (value < 0)
                    throw new ArgumentNullException(nameof(value));
                _coordinatesId = value;
            }
        }

        [DataMember]
        public Coordinates ObservationPoint { get; set; }    
       
        public override string ToString()
        {
            return
                $"ObservationPoint X - Y: {ObservationPoint.X.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture)}" +
                $" - {ObservationPoint.Y.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture)}," +
                $" Intensity: {_intensity}, Duration ms: {_intensity}," +
                $" Observation time: {_observationTime.Date:MM/dd/yyyy}, Estimated value: {EstimatedValue}";
        }

        public bool Equals(FlashObservation other)
        {
            return
                ObservationPoint.Equals(other.ObservationPoint)
                && ObservationTime.Equals(other._observationTime)
                && EstimatedValue.Equals(other.EstimatedValue);
        }

        public static bool operator ==(FlashObservation flashObservation1, FlashObservation flashObservation2)
        {
            return flashObservation1.ObservationPoint.Equals(flashObservation2.ObservationPoint)
                   && flashObservation1.ObservationTime.Equals(flashObservation2.ObservationTime)
                   && flashObservation1.EstimatedValue.Equals(flashObservation2.EstimatedValue);
        }

        public static bool operator !=(FlashObservation flashObservation1, FlashObservation flashObservation2)
        {
            return !flashObservation1.ObservationPoint.Equals(flashObservation2.ObservationPoint)
                   && !flashObservation1.ObservationTime.Equals(flashObservation2.ObservationTime)
                   && !flashObservation1.EstimatedValue.Equals(flashObservation2.EstimatedValue);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            return obj is FlashObservation observation && Equals(observation);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _intensity.GetHashCode();
                hashCode = (hashCode * 397) ^ _durationMs;
                hashCode = (hashCode * 397) ^ _observationTime.GetHashCode();
                hashCode = (hashCode * 397) ^ ObservationPoint.GetHashCode();
                return hashCode;
            }
        }
    }
}
