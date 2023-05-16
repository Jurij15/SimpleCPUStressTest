using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CPUStressTestUniversal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        void SetDefaultTheme()
        {
            if (Globals.Theme == 0)
            {
                ThemeComboBox.SelectedItem = DarkCombo;
            }
            else
            {
                ThemeComboBox.SelectedItem = LightCombo;
            }

            ElementSoundPlayer.State = Globals.SoundPlayerState;
            if (Globals.SoundPlayerState == ElementSoundPlayerState.On)
            {
                SoundToggle.IsOn = true;
            }
            else
            {
                SoundToggle.IsOn = false;
            }
        }
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SetDefaultTheme();
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ThemeComboBox.SelectedItem == DarkCombo)
            {
                Globals.Theme = 0;
                Globals.ThemeService.SetTheme();
            }
            else
            {
                Globals.Theme = 1;
                Globals.ThemeService.SetTheme();
            }
        }

        private void TempWarning_Toggled(object sender, RoutedEventArgs e)
        {

        }

        private void TempTestStop_Toggled(object sender, RoutedEventArgs e)
        {

        }

        private void SoundToggle_Toggled(object sender, RoutedEventArgs e)
        {
            bool state = ((ToggleSwitch)sender).IsOn;
            if (state == true)
            {
                ElementSoundPlayer.State = ElementSoundPlayerState.On;
                ElementSoundPlayer.State = Globals.SoundPlayerState;
            }
            else
            {
                ElementSoundPlayer.State = ElementSoundPlayerState.Off;
                ElementSoundPlayer.State = Globals.SoundPlayerState;
            }
        }
    }
}
