using LibreHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Wpf.Ui.Controls;

namespace SimpleCPUStressTestWPF_UI
{
    public class Config
    {
        public static Computer GComputer { get; set; }
        public static int TestRunningTime { get; set; }

        public static UiWindow MainWindow;

        public static CpuLoadGenerator CpuLoadGenerator { get; set; }
        public static int RefreshInterval = 1000; //in miliseconds

        public static Snackbar TestSnackbar { get; set; }
    }
}
