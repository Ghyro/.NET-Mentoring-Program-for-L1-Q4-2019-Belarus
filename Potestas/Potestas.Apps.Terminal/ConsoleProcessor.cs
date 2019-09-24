using System;

namespace Potestas.Apps.Terminal
{
    class ConsoleProcessor : IEnergyObservationProcessor
    {
        public string Description => "Logs all observations to console";

        public void OnCompleted()
        {
            Console.WriteLine("Processing completed");
        }

        public void OnError(Exception error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void OnNext(IEnergyObservation value)
        {
            Console.WriteLine(value);
        }
    }
}
