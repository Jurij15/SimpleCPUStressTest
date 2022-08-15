using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Windows.Threading;

namespace SimpleCPUStressTest
{
    public static class Test
    {
        static void loopForever()
        {
            while (true)
            {
                if (!Globals.bShouldTestBeRunning)
                {
                    break;                
                }
            }
        }
        public static void StopTest()
        {
            Globals.bShouldTestBeRunning = false;
            Console.WriteLine("Test stopped!");
        }
        public static void StartTest()
        {
            Console.WriteLine("Test started!");
            for (int ix = 0; ix < Environment.ProcessorCount; ++ix)
            {
                new Thread(loopForever).Start();
                if (!Globals.bShouldTestBeRunning)
                {
                    break;
                }
            }
        }
    }
}
