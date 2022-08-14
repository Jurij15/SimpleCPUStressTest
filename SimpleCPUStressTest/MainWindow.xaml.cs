using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
using ModernWpf;
using ModernWpf.Controls;

namespace SimpleCPUStressTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer LiveTime = new DispatcherTimer();
        Stopwatch stopwatch = new Stopwatch();
        void timer_tick(object sender, EventArgs e)
        {
            TimeSpan ts = stopwatch.Elapsed;
            Globals.time = (int)ts.TotalSeconds;
            TimerBox.Clear(); //incase there is something there already
            TimerBox.Text = Globals.time.ToString();
        }
        public MainWindow()
        {
            InitializeComponent();
            DynamicBtn.Content = "Start"; 
            Globals.bShouldTestBeRunning = false; 
            this.Topmost = true; 
            TimerBox.Clear(); 
            StatusBlock.Text = "Not Running";
        }

        private void DynamicBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!Globals.bShouldTestBeRunning)
            {
                DynamicBtn.Content = "Stop";
                Globals.bShouldTestBeRunning = true;
                stopwatch.Start();
                LiveTime.Interval = TimeSpan.FromSeconds(1);
                LiveTime.Tick += timer_tick;
                LiveTime.Start();
                StatusBlock.Text = "Running";
                Test.StartTest();
            }
            else if (Globals.bShouldTestBeRunning)
            {
                Test.StopTest();
                DynamicBtn.Content = "Start";
                StatusBlock.Text = "Not Running";
                LiveTime.Stop();
                stopwatch.Reset();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Globals.bShouldTestBeRunning = false;
            Environment.Exit(0);
        }
    }
}
