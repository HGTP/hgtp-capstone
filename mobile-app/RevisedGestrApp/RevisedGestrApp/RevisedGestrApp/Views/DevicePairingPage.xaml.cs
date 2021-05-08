/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RevisedGestrApp.ViewModels;
using Xamarin.Essentials;

namespace RevisedGestrApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DevicePairingPage : ContentPage
    {
        object pairingLock = new object();

        public DevicePairingPage()
        {
            this.BindingContext = new DevicePairingViewModel();
            InitializeComponent();
        }

        private async void OnScanClicked(object sender, EventArgs e)
        {
            DevicePairingViewModel model = (DevicePairingViewModel)this.BindingContext;
            ScanningIndicator.IsRunning = true;
            ScanningIndicator.IsVisible = true;
            ScanButton.IsEnabled = false;
            await model.ScanForDevices();
            if (model.PairedDevice == null)
            {
                lock (pairingLock)
                {
                    DeviceList.IsVisible = true;
                    DeviceList.IsEnabled = true;
                    ScanningIndicator.IsRunning = false;
                    ScanningIndicator.IsVisible = false;
                    ScanButton.IsEnabled = true;
                }
            }           
        }

        private async void OnConnectClicked(object sender, EventArgs e)
        {
            Button deviceButton = (Button)sender;
            UnpairButton.IsEnabled = true;
            deviceButton.IsEnabled = false;
            DeviceList.IsEnabled = false;
            ScanButton.IsEnabled = false;
            DevicePairingViewModel model = (DevicePairingViewModel)this.BindingContext;
            string deviceName = "HGTP";
            if (await model.ConnectToDevice(deviceName)) //, DisconnectedHanlder
            {
                lock (pairingLock)
                {
                    ScanButton.IsVisible = false;
                    DeviceList.IsVisible = false;
                    UnpairButton.IsVisible = true;
                    DeviceLabel.Text = "Connected to " + deviceName;
                    DeviceLabel.IsVisible = true;
                }
            }
            else
            {
                deviceButton.IsEnabled = true;
                ScanButton.IsEnabled = true;
                DeviceList.IsEnabled = true;
                DeviceLabel.Text = "Unable to connect to " + deviceName + ". Try again.";
                ResetDeviceModel();
            }
        }

        public async void OnUnpairClicked(object sender, EventArgs e)
        {
            DevicePairingViewModel model = (DevicePairingViewModel)this.BindingContext;
            UnpairButton.IsEnabled = false;
            if (await model.DisconnectDevice())
            {
                DisconnectedHanlder();
            }
            else
            {
                UnpairButton.IsEnabled = true;
                DeviceLabel.Text = "Unable to Disconnect. Try Again.";
            }
        }

        private bool DisconnectedHanlder()
        {
            lock (pairingLock)
            {
                UnpairButton.IsVisible = false;
                ScanButton.IsVisible = true;
                ScanButton.IsEnabled = true;
                DeviceLabel.Text = "Not Connected to a Device";
                DeviceLabel.IsVisible = true;
                ResetDeviceModel();
            }
            return true;
        }

        private void ResetDeviceModel()
        {
            this.BindingContext = new DevicePairingViewModel();
        }
    }
}