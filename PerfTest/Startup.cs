using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;

namespace PerfTest
{
    public class Startup : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }

        public void Start()
        {
            Console.WriteLine("Warming up...");
            for (int i = 0; i < 10; i++)
                Bus.SendLocal(new SimpleMessage());

            Thread.Sleep(10000);

            while (true)
            {
                Console.Write("Enter a number of messages to test:");
                string input = Console.ReadLine();
                Console.WriteLine();

                int target;
                if (int.TryParse(input, out target))
                {
                    Console.WriteLine("Starting Test of {0} messages...", target);
                    Stats.Start(target);
                    for (int i = 0; i < target; i++)
                        Bus.SendLocal(new SimpleMessage());

                    while (!Stats.Completed)
                        Thread.Sleep(5000);
                }

                
            }
        }

        public void Stop()
        {
            
        }
    }
}
