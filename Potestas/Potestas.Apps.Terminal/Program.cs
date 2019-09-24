using Potestas.Analizers;
using Potestas.Storages;
using System;

namespace Potestas.Apps.Terminal
{
    static class Program
    {
        private static readonly IEnergyObservationApplicationModel _app;
        private static ISourceRegistration _testRegistration;

        static Program()
        {
            _app = new ApplicationFrame();
        }

        static void Main()
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

    class ConsoleSourceFactory : ISourceFactory
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

    class ConsoleProcessingFactory : IProcessingFactory
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
