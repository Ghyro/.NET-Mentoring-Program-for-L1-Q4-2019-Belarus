using System;
using Potestas.Interfaces;

namespace Potestas.Apps.Terminal
{
    internal class ConsoleProcessor : IEnergyObservationProcessor
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
