using Potestas.Analizers;
using Potestas.Storages;
using System;
using Potestas.Interfaces;
using Potestas.Sources;

namespace Potestas.Apps.Terminal
{
    internal static class Program
    {
        private static readonly IEnergyObservationApplicationModel _app;
        private static ISourceRegistration _testRegistration;
        private static readonly RandomEnergySource _randomEnergySource;

        static Program()
        {
            _app = new ApplicationFrame();
            _randomEnergySource = new RandomEnergySource();
        }

        private static void Main()
        {
            Console.CancelKeyPress += Console_CancelKeyPress;

            _randomEnergySource.NewValueObserved += Show_NewValueObserved;
            _randomEnergySource.ObservationEnd += Show_ValueObservedDone;
            _randomEnergySource.ObservationError += Show_ValueObservedError;

            _testRegistration = _app.CreateAndRegisterSource(new ConsoleSourceFactory());
            _testRegistration.AttachProcessingGroup(new ConsoleProcessingFactory());
            _testRegistration.Start().Wait();
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Stopping application...");
            e.Cancel = true;
            _testRegistration.Stop();
        }

        private static void Show_NewValueObserved(object sender, IEnergyObservation e)
        {
            Console.WriteLine($"A new value observed: {e}");
        }

        private static void Show_ValueObservedDone(object sender, EventArgs e)
        {
            Console.WriteLine(e);
        }

        private static void Show_ValueObservedError(object sender, Exception e)
        {
            Console.WriteLine($"A new error has been generated: {e.Message}");
        }
    }

    internal class ConsoleSourceFactory : ISourceFactory
    {
        public IEnergyObservationEventSource CreateEventSource()
        {
            throw new NotImplementedException();
        }

        public IEnergyObservationSource CreateSource()
        {
            return new RandomEnergySource();
        }
    }

    internal class ConsoleProcessingFactory : IProcessingFactory
    {
        public IEnergyObservationAnalizer CreateAnalizer()
        {
            return new LINQAnalizer();
        }

        public IEnergyObservationProcessor CreateProcessor()
        {
            return new ConsoleProcessor();
        }

        public IEnergyObservationStorage CreateStorage()
        {
            return new ListStorage();
        }
    }
}
