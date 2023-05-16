using CPUStressTestUniversal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml;

namespace CPUStressTestUniversal
{
    public class Globals
    {
        public static ThemeService ThemeService { get; set; }

        public static int Theme = 0;

        public static ElementSoundPlayerState SoundPlayerState = ElementSoundPlayerState.Off;
    }
}
