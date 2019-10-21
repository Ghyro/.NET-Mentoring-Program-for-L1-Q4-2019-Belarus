using Potestas.Interfaces;
using Potestas.Observations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Potestas.Sources
{
    /* TASK. Implement random observation source.
     * 1. This class should generate observations by random periods of time.
     * 1. Implement both IEnergyObservationSource and IEnergyObservationSourceEventSource interfaces.
     * 2. Try to implement it with abstract class or delegate parameters to make it universal.
     */
    public class RandomEnergySource : IEnergyObservationSource
    {
        private readonly List<IObserver<IEnergyObservation>> _observers;
        private readonly List<IEnergyObservation> _energyObservations;

        public RandomEnergySource()
        {
            _observers = new List<IObserver<IEnergyObservation>>();
            _energyObservations = new List<IEnergyObservation>();
        }

        public string Description => "Console RandomEnergySource";

        public Task Run(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {                    
                    var randomPeriod = new Random((int)DateTime.Now.Ticks);
                    var newObservation = RandomObservationServices.CreateRandomObservation();
                    _energyObservations.Add(newObservation);
                    GenerateRandomObservation(newObservation);
                    Thread.Sleep(randomPeriod.Next(100, 1000));                    
                }
            }, cancellationToken);
        }

        private void GenerateRandomObservation(FlashObservation newObservation)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(newObservation);
            }
        }

        public IDisposable Subscribe(IObserver<IEnergyObservation> observer)
        {
            if (!_observers.Contains(observer))            
                _observers.Add(observer);            

            return new Unsubscriber(_observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private readonly List<IObserver<IEnergyObservation>> _observers;
            private readonly IObserver<IEnergyObservation> _observer;

            public Unsubscriber(List<IObserver<IEnergyObservation>> observers, IObserver<IEnergyObservation> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observers != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
    }
}
