using Potestas.Interfaces;
using Potestas.Sources.Events;
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
    public class RandomEnergySource : IEnergyObservationSource, IEnergyObservationEventSource
    {
        private readonly List<IObserver<IEnergyObservation>> _processors;

        public RandomEnergySource()
        {
            _processors = new List<IObserver<IEnergyObservation>>();
        }

        public string Description => "Console RandomEnergySource";

        public async Task Run(CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var str = Console.ReadLine();
                        if (!string.IsNullOrEmpty(str))
                        {
                            GenerateNewFlashObservation();
                        }
                    }
                    catch (Exception ex)
                    {
                        PublishException(ex);
                        throw;
                    }
                }
            });
        }

        public void GenerateNewFlashObservation()
        {
            var flashObservation = RandomObservationServices.CreateRandomObservation();

            foreach (var processor in _processors)
            {          
                processor.OnNext(flashObservation);

                var args = new NewValueObservedEventArgs(flashObservation);

                OnNewValueObserved(args.FlashObservation);
            }
        }

        public IDisposable Subscribe(IObserver<IEnergyObservation> processor)
        {
            if (!_processors.Contains(processor))
                _processors.Add(processor);
            return new Unsubscriber(_processors, processor);
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<IEnergyObservation>> _processors;
            private IObserver<IEnergyObservation> _processor;

            public Unsubscriber(List<IObserver<IEnergyObservation>> observers, IObserver<IEnergyObservation> observer)
            {
                _processors = observers;
                _processor = observer;
            }

            public void Dispose()
            {
                if (_processor != null && _processors.Contains(_processor))
                    _processors.Remove(_processor);
            }
        }        

        public async Task CheckCancellation(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(500);
            }
            Done();
        }

        private void Done()
        {
            foreach (var processor in _processors)
            {
                processor.OnCompleted();

                var args = new ValueObservationDoneEventArgs("A new value observation done");

                OnValueObservationDone(args);
            }
        }

        private void PublishException(Exception error)
        {
            foreach (var processor in _processors)
            {
                processor.OnError(error);

                var args = new ValueObservationErrorEventArgs(error);

                OnValueObservationError(args.Exception);
            }
        }

        #region events

        protected virtual void OnNewValueObserved(IEnergyObservation e)
        {
            NewValueObserved?.Invoke(this, e);
        }

        protected virtual void OnValueObservationDone(ValueObservationDoneEventArgs e)
        {
            ObservationEnd?.Invoke(this, e);
        }

        protected virtual void OnValueObservationError(Exception e)
        {
            ObservationError?.Invoke(this, e);
        }

        public event EventHandler<IEnergyObservation> NewValueObserved;
        public event EventHandler<Exception> ObservationError;
        public event EventHandler ObservationEnd;

        #endregion
    }
}
