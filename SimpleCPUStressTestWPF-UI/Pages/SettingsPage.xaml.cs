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

namespace SimpleCPUStressTestUI.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Wpf.Ui.Controls.UiPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            Wpf.Ui.Appearance.Accent.Apply(Colors.LightGreen);

            if (Wpf.Ui.Appearance.Theme.GetAppTheme() == Wpf.Ui.Appearance.ThemeType.Dark)
            {
                DarkRadio.IsChecked = true;
                LightRadio.IsChecked = false;
            }
            else
            {
                DarkRadio.IsChecked = false;
                LightRadio.IsChecked = true;
            }

            MicaBackdrop.IsChecked = true;
        }

        private void LightRadio_Checked(object sender, RoutedEventArgs e)
        {
            Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Light);
            Wpf.Ui.Appearance.Accent.Apply(Colors.LightGreen);
        }

        private void DarkRadio_Checked(object sender, RoutedEventArgs e)
        {
            Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Dark);
            Wpf.Ui.Appearance.Accent.Apply(Colors.LightGreen);
        }

        private void CPUTempWarningsSwitch_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CPUTempWarningsSwitch_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void CPUTempStopTest_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CPUTempStopTest_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void MicaBackdrop_Checked(object sender, RoutedEventArgs e)
        {
            Config.MainWindow.WindowBackdropType = Wpf.Ui.Appearance.BackgroundType.Mica;
            MicaAltBackDrop.IsChecked = false;
        }

        private void MicaAltBackDrop_Checked(object sender, RoutedEventArgs e)
        {
            Config.MainWindow.WindowBackdropType = Wpf.Ui.Appearance.BackgroundType.Tabbed;
            MicaBackdrop.IsChecked = false;
        }
    }
}
