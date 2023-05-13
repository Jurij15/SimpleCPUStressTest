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
using LibreHardwareMonitor.Hardware.Cpu;
using System.ComponentModel;

namespace SimpleCPUStressTestUI.Pages
{
    /// <summary>
    /// Interaction logic for TestPage.xaml
    /// </summary>
    public partial class TestPage : Wpf.Ui.Controls.UiPage
    {
        private readonly PerformanceCounter _cpuCounter;
        public Computer Computer { get; private set; }
        public System.Timers.Timer _timer;
        public DispatcherTimer RunningTimer;
        BackgroundWorker refresherWorker = new BackgroundWorker();
        void SetCPUUsage()
        {
            UsageProgressBar.Maximum = 100;
            // Get the current CPU usage percentage
            float cpuUsage = _cpuCounter.NextValue();

            // Update the TextBlock with the new value
            UsageValue.Text = string.Format("{0:F0}%", cpuUsage);
            UsageProgressBar.Value = cpuUsage;
        }

        void SetCPUUsageNew()
        {
            foreach (var hardware in Config.GComputer.Hardware)
            {
                hardware.Update();

                if (hardware.HardwareType == HardwareType.Cpu)
                {
                    hardware.Update();
                    foreach (var sensor in hardware.Sensors)
                    {
                        hardware.Update();
                        if (sensor.SensorType == SensorType.Load && sensor.Name.Contains("Total"))
                        {
                            Application.Current.Dispatcher.Invoke(()=>
                            {
                                float value = sensor.Value.GetValueOrDefault();

                                UsageProgressBar.Maximum = 100;
                                UsageProgressBar.Value = value;

                                UsageValue.Text = string.Format("{0:F0}%", Convert.ToInt32(Convert.ToInt32(value)));
                            });
                        }
                    }
                }
            }

            //Config.GComputer.Reset();
        }

        void SetCPUTemp()
        {
            IHardware cpuHardware = Config.GComputer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);

            // Find the first temperature sensor of the CPU
            ISensor cpuTemperatureSensor = cpuHardware?.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Temperature && s.Name.StartsWith("Core "));

            // Display the current CPU temperature in the text block
            Application.Current.Dispatcher.Invoke(() =>
            {
                int MaxTemp = 90;
                TemperatureProgressBar.Maximum = MaxTemp;

                if (cpuTemperatureSensor != null)
                {
                    float cpuTemperature = (float)cpuTemperatureSensor.Value;
                    TemperatureValue.Text = string.Format("{0:F0}°C", cpuTemperature);

                    TemperatureProgressBar.Value = cpuTemperature;

                    if (cpuTemperature > MaxTemp)
                    {
                        TemperatureProgressBar.Foreground = Brushes.Red;
                        CPUTempHigh.IsOpen = true;
                    }
                    else if (cpuTemperature >= 100)
                    {
                        TestStopBtn_Click(null, null);
                    }
                    else
                    {
                        TemperatureProgressBar.Foreground = UsageProgressBar.Foreground;
                        CPUTempHigh.IsOpen = false;
                    }
                }
                else
                {
                    TemperatureValue.Text = "%";
                }
            });

            //Config.GComputer.Reset();
        }

        void SetCPUClock()
        {
            var CpuHardware = Config.GComputer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);

            var sensor = CpuHardware.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Clock && s.Name.Contains("Core"));

            int value = Convert.ToInt32(sensor.Value);

            Application.Current.Dispatcher.Invoke(() =>
            {
                ClockProgressBar.Maximum = 5000;

                ClockProgressBar.Value = value;

                ClockValue.Text = string.Format("{0:F0}MHz", value);
            });
        }

        void SetCPUPower()
        {
            int allcoresusage = 0;
            foreach (var hardware in Config.GComputer.Hardware)
            {
                hardware.Update();

                if (hardware.HardwareType == HardwareType.Cpu)
                {
                    hardware.Update();
                    foreach (var sensor in hardware.Sensors)
                    {
                        hardware.Update();
                        if (sensor.SensorType == SensorType.Power)
                        {
                            hardware.Update();

                            allcoresusage += Convert.ToInt32(sensor.Value.Value);

                            hardware.Update() ;
                        }
                    }
                }
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                PowerProgressBar.Maximum = Convert.ToInt32(150);
                PowerProgressBar.Value = allcoresusage;

                PowerValue.Text = string.Format("{0:F0}W", Convert.ToInt32(Convert.ToInt32(allcoresusage)));
            });
            //Config.GComputer.Reset();
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
            //Thread.Sleep(1000);
            Config.GComputer = new Computer();
            Config.GComputer.IsCpuEnabled = true;
            Config.GComputer.IsGpuEnabled = true;
            Config.GComputer.IsMemoryEnabled = true;
            Config.GComputer.IsMotherboardEnabled = true;
            Config.GComputer.IsControllerEnabled = true;
            Config.GComputer.Open();
            Config.GComputer.Accept(new UpdateVisitor());

            refresherWorker = new BackgroundWorker();
            refresherWorker.DoWork += (sender, e) =>
            {
                _timer = new System.Timers.Timer(Config.RefreshInterval);
                _timer.Interval = Config.RefreshInterval;
                _timer.Elapsed+= (s, args)=>
                {
                    SetCPUUsageNew();
                    SetCPUTemp();
                    SetCPUClock();
                    SetCPUPower();
                };
                _timer.Start();
            };
            refresherWorker.RunWorkerAsync();

            string CpuName = null;
            foreach (IHardware hardware in Config.GComputer.Hardware)
            {
                if (hardware.HardwareType == HardwareType.Cpu)
                {
                    CpuName = hardware.Name;

                    break;
                }
            }

            CPUNameBlock.Text = CpuName;

            //CPUBrandImage.Source = 
        }

        public void StartRunningTimer()
        {
            RunningTimer = new DispatcherTimer();
            RunningTimer.Interval = TimeSpan.FromSeconds(1);
            RunningTimer.Tick += RunningTimerTick;
            RunningTimer.Start();
        }

        void RunningTimerTick(object sender, EventArgs e)
        {
            Config.TestRunningTime++;
            TestStatus.Text = $"Running, Running for {Config.TestRunningTime} seconds";

            Config.TestSnackbar.Title = "Test running";
            Config.TestSnackbar.Message = $"Running for {Config.TestRunningTime} seconds";

            Config.TestSnackbar.CloseButtonEnabled = false;
            Config.TestSnackbar.ToolTip = "Displays information about current test";
            Config.TestSnackbar.Timeout = 1000000;

            Config.TestSnackbar.Visibility = Visibility.Visible;
        }

        private void UiPage_Unloaded(object sender, RoutedEventArgs e)
        {
            //_timer.Stop(); //only stop this timer, so we do not hang the application
        }

        private void TestStartBtn_Click(object sender, RoutedEventArgs e)
        {
            StartRunningTimer();
            ((Wpf.Ui.Controls.Button)sender).Visibility = Visibility.Collapsed;
            TestStopBtn.Visibility = Visibility.Visible;

            Config.CpuLoadGenerator = new CpuLoadGenerator();
            Config.CpuLoadGenerator.Start(int.Parse(CPUUsageUserSetting.Text));

            TestSettingsExpander.IsExpanded = false;
        }

        private void TestStopBtn_Click(object sender, RoutedEventArgs e)
        {
            RunningTimer.Stop();
            ((Wpf.Ui.Controls.Button)sender).Visibility = Visibility.Collapsed;
            TestStartBtn.Visibility = Visibility.Visible;

            TestStatus.Text = $"Stopped, Ran for {Config.TestRunningTime} seconds";

            Config.TestRunningTime = 0;

            Config.CpuLoadGenerator.Stop();

            //Config.TestSnackbar.Hide();
            Config.TestSnackbar.Visibility = Visibility.Collapsed;

            TestSettingsExpander.IsExpanded = true;
        }

        private void SaveTestToggle_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void SaveTestToggle_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }
}
