using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Management;
using LibreHardwareMonitor.Hardware;
using System.Collections;
using SimpleCPUStressTestWPF_UI;
using Windows.Media.Core;
using System.Threading.Tasks.Sources;

namespace SimpleCPUStressTestUI.Pages
{
    /// <summary>
    /// Interaction logic for TestPage.xaml
    /// </summary>
    public partial class TestPage : Wpf.Ui.Controls.UiPage
    {
        private readonly PerformanceCounter _cpuCounter;
        public Computer Computer { get; private set; }
        public DispatcherTimer _timer;
        public DispatcherTimer RunningTimer;

        void SetCPUUsage()
        {
            CPUUsageProgressbar.Maximum = 100;
            // Get the current CPU usage percentage
            float cpuUsage = _cpuCounter.NextValue();

            // Update the TextBlock with the new value
            CPUUsageBlock.Text = string.Format("{0:F0}%", cpuUsage);
            CPUUsageProgressbar.Value = cpuUsage;
        }

        void SetCPUUsageNew()
        {
            CPUUsageProgressbar.Maximum = 100;
            var usage = _cpuCounter.NextValue();

            // Get the CPU load sensor and its current value
            var cpuLoadSensor = Config.GComputer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu)?.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Load && s.Name == "CPU Total");
            var cpuLoad = cpuLoadSensor?.Value ?? 0;

            // Calculate the CPU usage percentage
            var cpuUsagePercentage = cpuLoad > 0 ? usage / cpuLoad * 100 : 0;

            // Update the CPU usage property
            CPUUsageBlock.Text = cpuUsagePercentage.ToString();
        }

        void SetCPUTemp()
        {
            IHardware cpuHardware = Config.GComputer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);

            // Find the first temperature sensor of the CPU
            ISensor cpuTemperatureSensor = cpuHardware?.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Temperature && s.Name.StartsWith("Core "));

            int MaxTemp = 90;
            CPUTempProgressBar.Maximum = MaxTemp;

            // Display the current CPU temperature in the text block
            if (cpuTemperatureSensor != null)
            {
                float cpuTemperature = (float)cpuTemperatureSensor.Value;
                CPUTempBlock.Text = string.Format("{0:F0}°C", cpuTemperature);

                CPUTempProgressBar.Value = cpuTemperature;

                if (cpuTemperature > MaxTemp)
                {
                    CPUTempProgressBar.Foreground = Brushes.Red;
                }
                else
                {
                    CPUTempProgressBar.Foreground = CPUUsageProgressbar.Foreground;
                }
            }
            else
            {
                CPUTempBlock.Text = "%";
            }

            Config.GComputer.Reset();
        }

        void UpdateAllStats()
        {
            SetCPUTemp();
            SetCPUUsage();
        }

        public void RefreshTimerTick(object sender, EventArgs e)
        {
            UpdateAllStats();
        }

        public TestPage()
        {
            InitializeComponent();

            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }

        public class UpdateVisitor : IVisitor
        {
            public void VisitComputer(IComputer computer)
            {
                computer.Traverse(this);
            }
            public void VisitHardware(IHardware hardware)
            {
                hardware.Update();
                foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
            }
            public void VisitSensor(ISensor sensor) { }
            public void VisitParameter(IParameter parameter) { }
        }

        private void UiPage_Loaded(object sender, RoutedEventArgs e)
        {
            Config.GComputer = new Computer();
            Config.GComputer.IsCpuEnabled = true;
            Config.GComputer.IsGpuEnabled = true;
            Config.GComputer.IsMemoryEnabled = true;
            Config.GComputer.IsMotherboardEnabled = true;
            Config.GComputer.IsControllerEnabled = true;
            Config.GComputer.Open();
            Config.GComputer.Accept(new UpdateVisitor());

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(800);
            _timer.Tick += RefreshTimerTick;
            _timer.Start();
        }

        private void UiPage_Unloaded(object sender, RoutedEventArgs e)
        {
            _timer.Stop(); //only stop this timer, so we do not hang the application
        }
    }
}
