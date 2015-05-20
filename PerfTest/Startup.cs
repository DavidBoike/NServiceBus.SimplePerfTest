using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace PerfTest
{
    public class Startup : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }

        public void Start()
        {
            Console.WriteLine("Press any key to start/stop test.");
            while (true)
            {
                Console.WriteLine("Currently STOPPED");
                Console.ReadKey(true);

                Console.WriteLine("Starting Test...");
                Stats.Start();
                while (!Console.KeyAvailable)
                {
                    Bus.SendLocal(new SimpleMessage());
                }
                Console.ReadKey(true);
                Stats.StopAndReport();
            }
        }

        public void Stop()
        {
            
        }
    }
}
