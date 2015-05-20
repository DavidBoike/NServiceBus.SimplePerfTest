using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace PerfTest
{
    public class SimpleHandler : IHandleMessages<SimpleMessage>
    {
        public void Handle(SimpleMessage message)
        {
            Stats.Increment();
        }
    }
}
