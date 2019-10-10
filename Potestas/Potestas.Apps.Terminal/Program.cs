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
