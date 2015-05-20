using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Configuration.AdvanceExtensibility;

namespace PerfTest
{
    public static class Stats
    {
        private static int counter = 0;
        private static int targetCount = -1;
        private static Stopwatch watch = new Stopwatch();
        private static BusConfiguration busConfig;

        public static void Initialize(BusConfiguration cfg)
        {
            busConfig = cfg;
        }

        public static void Start(int target)
        {
            Completed = false;
            counter = 0;
            targetCount = target;
            watch.Reset();
            watch.Start();
        }

        public static bool Completed { get; private set; }

        public static void Increment()
        {
            int newValue = Interlocked.Increment(ref counter);

            if (newValue < targetCount)
            {
                if (newValue % 100 == 0) 
                    Console.Write("\rProgress: {0}/{1}", counter, targetCount);
                return;
            }

            if (targetCount < 0)
                return;
            
            watch.Stop();
            Console.WriteLine();

            var transport = busConfig.GetSettings().GetConfigSection<TransportConfig>();
            double throughput = (double) counter/watch.Elapsed.TotalSeconds;
            double throughputPerThread = throughput/transport.MaximumConcurrencyLevel;

            Console.WriteLine("=====================");
            Console.WriteLine("=== TEST RESULTS ===");
            Console.WriteLine("=====================");
            Console.WriteLine("Messages Processed: {0}", targetCount);
            Console.WriteLine("Elapsed Time: {0}", watch.Elapsed);
            Console.WriteLine("Receive Threads: {0}", transport.MaximumConcurrencyLevel);
            Console.WriteLine("Msgs/second: {0}", throughput);
            Console.WriteLine("Msgs/thread/sec: {0}", throughputPerThread);
            Console.WriteLine("=====================");
            Console.WriteLine();

            Completed = true;
        }
    }
}
