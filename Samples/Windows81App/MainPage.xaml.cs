﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Windows81App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

           
            System.Diagnostics.Debug.WriteLine(Package.Current.DisplayName);
            System.Diagnostics.Debug.WriteLine(Package.Current.Id.Name);
            System.Diagnostics.Debug.WriteLine(Package.Current.Id.Version);
            //System.Diagnostics.Debug.WriteLine(Package.Current.InstalledDate);
            System.Diagnostics.Debug.WriteLine(Package.Current.IsDevelopmentMode);
            System.Diagnostics.Debug.WriteLine(InTheHand.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamilyVersion);

            System.Diagnostics.Debug.WriteLine(Windows.ApplicationModel.Package.Current);
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            Windows.UI.ApplicationSettings.SettingsPane.Show();
        }
    }
}
