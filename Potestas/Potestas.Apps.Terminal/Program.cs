using System;
using Potestas.Interfaces;
using Potestas.Sources;

namespace Potestas.Apps.Terminal
{
    internal static class Program
    {
        private static readonly IEnergyObservationApplicationModel _app;
        private static ISourceRegistration _testRegistration;

        static Program()
        {
            _app = new ApplicationFrame();
        }

        private static void Main()
        {
            Console.CancelKeyPress += Console_CancelKeyPress;
            _testRegistration = _app.CreateAndRegisterSource(new ConsoleSourceFactory());
            _testRegistration.AttachProcessingGroup(new ConsoleProcessingFactory(), null, null);
            _testRegistration.Start().Wait();
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Stopping application...");
            e.Cancel = true;
            _testRegistration.Stop();
        }
    }

    internal class ConsoleSourceFactory : ISourceFactory<IEnergyObservation>
    {
        public IEnergyObservationEventSource<IEnergyObservation> CreateEventSource()
        {
            throw new NotImplementedException();
        }

        public IEnergyObservationSource<IEnergyObservation> CreateSource()
        {
            return new RandomEnergySource<IEnergyObservation>();
        }
    }

    internal class ConsoleProcessingFactory : IProcessingFactory<IEnergyObservation>
    {
        public IEnergyObservationProcessor<IEnergyObservation> CreateProcessor(IStorageFactory<IEnergyObservation> storageFactory = null, IProcessingFactory<IEnergyObservation> processorFactory = null)
        {
            return new ConsoleProcessor();
        }
    }
}
