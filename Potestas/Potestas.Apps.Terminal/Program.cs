using Potestas.Analizers;
using Potestas.Storages;
using System;
using Potestas.Interfaces;

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
            return new ConsoleSource();
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
