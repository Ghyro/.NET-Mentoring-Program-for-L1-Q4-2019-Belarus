using Potestas.Observations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Potestas.Interfaces;
using Potestas.Sources;
using Potestas.Sources.Events;

namespace Potestas.Apps.Terminal
{
    class ConsoleSourceSubscription : IDisposable
    {
        private readonly ConsoleSource _source;
        private readonly IObserver<IEnergyObservation> _processor;

        public ConsoleSourceSubscription(ConsoleSource source, IObserver<IEnergyObservation> processor)
        {
            _source = source;
            _processor = processor;
        }

        public void Dispose()
        {
            _source.Unsubscribe(_processor);
        }
    }

    class ConsoleSource : IEnergyObservationSource
    {
        private readonly List<IObserver<IEnergyObservation>> _processors;
        private readonly RandomEnergySource _randomEnergySource;

        public string Description => "Console input energy observation";

        public ConsoleSource()
        {
            _processors = new List<IObserver<IEnergyObservation>>();
            _randomEnergySource = new RandomEnergySource();
        }

        public async Task Run(CancellationToken cancellationToken)
        {        
            cancellationToken.ThrowIfCancellationRequested();
            await Task.WhenAny(
                _randomEnergySource.Run(cancellationToken),
                _randomEnergySource.CheckCancellation(cancellationToken)
                );
        }        

        public IDisposable Subscribe(IObserver<IEnergyObservation> observer)
        {
            _processors.Add(observer);
            return new ConsoleSourceSubscription(this, observer);
        }

        internal void Unsubscribe(IObserver<IEnergyObservation> observer)
        {
            _processors.Remove(observer);
        }      
    }
}
