using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using System.Diagnostics;
using BTPairing.Models;

namespace BTPairing.ViewModels
{
    public class BLE_ViewModel : INotifyPropertyChanged
    {
        ObservableCollection<DeviceObj> devices;
        public event PropertyChangedEventHandler PropertyChanged;
        private IBluetoothLE ble;
        private string ble_state;
        private IAdapter adapter;
        private List<IDevice> deviceList;
        public string IsPairedWith { get; set; }

        public ObservableCollection<DeviceObj> Devices { get { return devices; } }

        public ICommand ScanDeviceCommand { protected set; get; }

        public Command<string> ConnectDeviceCommand { protected set; get; }

        public string BLE_state
        {
            get { return ble_state; }
            set
            {
                if (ble_state != value)
                {
                    ble_state = value;
                    OnPropertyChanged("BLE_state");
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public BLE_ViewModel()
        {
            deviceList = new List<IDevice>();
            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            devices = new ObservableCollection<DeviceObj>();

            //Delegate to set BLE_state (bound to label in Mainpage.xaml) whenever ble.StateChanged event fires
            ble.StateChanged += (s, e) =>
            {
                BLE_state = e.NewState.ToString();
            };
            BLE_state = ble.State.ToString();

            //Delegates to add discovered devices & names to deviceList & DeviceNames, respectively, during scan
            adapter.DeviceDiscovered += (s, a) =>
            {
                bool j = false;
                foreach (IDevice i in deviceList)
                {
                    if (i.Name == a.Device.Name)
                    {
                        j = true;
                        break;
                    }
                }
                if (!j)
                {
                    deviceList.Add(a.Device);
                    devices.Add(new DeviceObj { DeviceName = a.Device.Name });
                }
            };

            ScanDeviceCommand = new Command(
                () => { ScanForDevices(); });

            ConnectDeviceCommand = new Command<string>(
                execute: (string arg) =>
                {
                    foreach (IDevice i in deviceList)
                    {
                        if (i.Name == arg)
                        {
                            ConnectToDevice(i);
                            break;
                        }
                    }
                });
        }

        private async void ScanForDevices()
        {
            await adapter.StartScanningForDevicesAsync();
            OnPropertyChanged("Devices");
        }

        private async void ConnectToDevice(IDevice i)
        {
            try
            {
                await adapter.ConnectToDeviceAsync(i);
                OnPropertyChanged("BLE_state");
                Debug.WriteLine("Connected!");
                IsPairedWith = i.Name;
                OnPropertyChanged("IsPairedWith");
                var services = await i.GetServicesAsync();
                Debug.WriteLine("Services: ");
                foreach (IService s in services)
                {
                    Debug.WriteLine(s.Name);
                    Debug.WriteLine("    Characteristics:");
                    var characteristics = await s.GetCharacteristicsAsync();
                    foreach (ICharacteristic c in characteristics)
                    {
                        Debug.WriteLine("    " + c.Name);
                    }
                }
                System.Threading.Thread.Sleep(10000);
                services = await i.GetServicesAsync();
                Debug.WriteLine("Services: ");
                foreach (IService s in services)
                {
                    Debug.WriteLine(s.Name);
                    Debug.WriteLine("    Characteristics:");
                    var characteristics = await s.GetCharacteristicsAsync();
                    foreach (ICharacteristic c in characteristics)
                    {
                        Debug.WriteLine("    " + c.Name);
                    }
                }
            }
            catch (DeviceConnectionException e)
            {
                Debug.WriteLine("Could not connect to device");
            }
        }
    }
}
