using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Windows.Threading;
using System.Runtime.InteropServices;

namespace SimpleCPUStressTest
{
    public static class Globals
    {
        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole();
        public static bool bShouldTestBeRunning;
        public static int time;
        public static bool bEnableLogging = true;
        public static void SetupConsole()
        {
            AllocConsole();
            Console.WriteLine("Console Enabled!");
        }
    }
}
