using CPUStressTestUniversal.Services;
using Microsoft.Toolkit.Uwp.UI.Media;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CPUStressTestUniversal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        void SetTitleBar()
        {
            var titleBar = CoreApplication.GetCurrentView().TitleBar;
            titleBar.ExtendViewIntoTitleBar = true;

            Window.Current.SetTitleBar(TitleBarGrid);
        }

        void ApplyBackdrop()
        {
            if (Environment.OSVersion.Version.Build >= 22000)
            {
                SetValue(BackdropMaterial.ApplyToRootOrPageBackgroundProperty, true);
            }

            Globals.ThemeService = new ThemeService(Window.Current);
            Globals.ThemeService.SetTheme();
        }

        public MainPage()
        {
            this.InitializeComponent();
            SetTitleBar();
            ApplyBackdrop();
        }
    }
}
