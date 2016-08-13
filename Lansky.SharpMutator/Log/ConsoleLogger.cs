using System;
using System.Diagnostics;

namespace Lansky.SharpMutator.Log
{
    sealed class ConsoleLogger : ILogger
    {
        private Stopwatch TimeFromStart;

        public ConsoleLogger()
        {
            TimeFromStart = new Stopwatch();
            TimeFromStart.Start();
        }

        public void Info(string info)
        {
            Console.WriteLine($"{TimeFromStart.ElapsedTicks/10000.0:00,000.00} ms: {info}");
        }
    }
}
