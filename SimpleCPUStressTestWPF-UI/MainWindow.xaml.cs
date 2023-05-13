using SimpleCPUStressTestUI.Pages;
using SimpleCPUStressTestWPF_UI;
using System;
using System.Collections.Generic;
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

namespace SimpleCPUStressTestUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Wpf.Ui.Controls.UiWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            RootNavigation.Navigate(typeof(TestPage));

            Wpf.Ui.Appearance.Accent.Apply(Colors.LightGreen);

            Config.TestSnackbar = TestSnackbar;

            TestSnackbar.Visibility = Visibility.Collapsed;
            Config.TestSnackbar.Show();

            Config.MainWindow = this;
        }

        private void FluentWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UiWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Config.GComputer != null)
            {
                Config.GComputer.Close();
            }
        }

        private void TestSnackbar_Closed(Wpf.Ui.Controls.Snackbar sender, RoutedEventArgs e)
        {

        }
    }
}
