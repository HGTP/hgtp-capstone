/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using RevisedGestrApp.Models;
using RevisedGestrApp.Util;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Math;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.IO;

namespace RevisedGestrApp.ViewModels
{
    class DevicePairingViewModel : BaseViewModel
    {
        /// <summary>
        /// Needed for communication with the view.
        /// </summary>
        public IDevice PairedDevice { get; protected set; }
        public bool IsNotScanning { get; private set; }
        public ObservableCollection<BluetoothDevice> Devices { get { return devices; } }

        /// <summary>
        /// Needed for Bluetooth pairing/deivce management. 
        /// </summary>
        ObservableCollection<BluetoothDevice> devices;
        private IBluetoothLE ble;
        private string bluetoothState;
        private IAdapter adapter;
        private List<IDevice> deviceList;
        private ICharacteristic characteristic;

        /// <summary>
        /// Needed for secure encryption communication.
        /// </summary>
        private static string ArdPubKeyStr;
        private static string EncryptedMsg;
        private static BigInteger sharedKey;

        public DevicePairingViewModel()
        {
            ArdPubKeyStr = "";
            EncryptedMsg = "";
            deviceList = new List<IDevice>();
            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            devices = new ObservableCollection<BluetoothDevice>();
            IsNotScanning = true;
            PairedDevice = null;
            //Delegate to set BluetoothState (bound to label in DevicePairingPage.xaml) whenever ble.StateChanged event fires
            ble.StateChanged += (s, e) =>
            {
                BluetoothState = e.NewState.ToString();
            };
            BluetoothState = ble.State.ToString();

            //Delegates to add discovered devices & names to deviceList & DeviceNames, respectively, during scan
            adapter.DeviceDiscovered += (s, a) =>
            {
                Console.WriteLine(a.Device.Name);
                if (a.Device.Name == "HGTP" && deviceList.Count == 0)
                {
                    deviceList.Add(a.Device);
                    devices.Add(new BluetoothDevice { Name = a.Device.Name });
                }
            };

            adapter.DeviceConnected += (s, a) =>
            {
                PairedDevice = a.Device;
            };
        }

        /// <summary>
        /// Scans for devices with a scan timeout of 500(time units?).
        /// </summary>
        public async Task<bool> ScanForDevices()
        {
            IsNotScanning = false;
            adapter.ScanTimeout = 5000;
            await adapter.StartScanningForDevicesAsync();
            await adapter.StartScanningForDevicesAsync();
            OnPropertyChanged("Devices");
            return true;
        }

        /// <summary>
        /// Connects to the specified device and completes encryption handshake/key generation. 
        /// </summary>
        /// <param name="deviceName">Name of the device to connect to.</param>
        public async Task<bool> ConnectToDevice(string deviceName)  //, Func<bool> DisconnectedHanlder
        {
            //adapter.DeviceConnectionLost += (s, a) =>
            //{
            //    DisconnectedHanlder();
            //};

            try
            {
                IDevice device = null;
                foreach (IDevice i in deviceList)
                {
                    if (i.Name == deviceName)
                    {
                        device = i;
                        break;
                    }
                }
                await adapter.ConnectToDeviceAsync(device);
                OnPropertyChanged("BluetoothState");
                PairedDevice = device;
                var service = await device.GetServiceAsync(Guid.Parse("913CF3FD-7173-43A5-82F4-DFD6F61BAF5F"));
                characteristic = await service.GetCharacteristicAsync(Guid.Parse("44B1DF4E-15C8-4F97-9F34-123D33B0C29D"));

                X9ECParameters x9 = ECNamedCurveTable.GetByName("secp256r1");
                ECCurve curve = x9.Curve;
                ECDomainParameters ecDomain = new ECDomainParameters(x9.Curve, x9.G, x9.N, x9.H, x9.GetSeed());
                ECKeyPairGenerator generator = (ECKeyPairGenerator)GeneratorUtilities.GetKeyPairGenerator("ECDH");
                generator.Init(new ECKeyGenerationParameters(ecDomain, new SecureRandom()));
                AsymmetricCipherKeyPair appKeyPair = generator.GenerateKeyPair();
                ECPublicKeyParameters appPublicKey = (ECPublicKeyParameters)appKeyPair.Public;
                ECPrivateKeyParameters appPrivateKey = (ECPrivateKeyParameters)appKeyPair.Private;


                // Wait for Arduino Public Key
                characteristic.ValueUpdated += GetPubKeyPortion;
                await characteristic.StartUpdatesAsync();
                Task.Run(CheckIfKeyObtainComplete).Wait();

                // Once full Key has been obtained, stop reading characteristic and unregister handler
                await characteristic.StopUpdatesAsync();
                characteristic.ValueUpdated -= GetPubKeyPortion;

                // Cut off 'starting' padding from beginning of arduino's public key & convert to byte array
                ArdPubKeyStr = ArdPubKeyStr.Substring(8);

                // Now send the app's public key to arduino via same characteristic
                string appPublicKeyStr = appPublicKey.Q.ToString();
                string appPubKey_x = appPublicKey.Q.XCoord.ToString();
                string appPubKey_y = appPublicKey.Q.YCoord.ToString();

                for (int i = 0; i < 64 - appPubKey_x.Length; i++)
                    appPubKey_x = "0" + appPubKey_x;
                for (int i = 0; i < 64 - appPubKey_y.Length; i++)
                    appPubKey_y = "0" + appPubKey_y;

                appPublicKeyStr = appPubKey_x + appPubKey_y;
                char[] pubkey_chars = appPublicKeyStr.ToCharArray();
                byte[] pubkey_bytes = new byte[128];

                for (int i = 0; i < 128; i++)
                    pubkey_bytes[i] = Convert.ToByte(pubkey_chars[i]);

                byte[][] chunks = pubkey_bytes
                    .Select((c, i) => new { Value = c, Index = i })
                    .GroupBy(x => x.Index / 16)
                    .Select(grp => grp.Select(x => x.Value).ToArray())
                    .ToArray();

                foreach (byte[] b in chunks)
                    await characteristic.WriteAsync(b);

                BigInteger Q_x = new BigInteger(1, StringToByteArray(ArdPubKeyStr.Substring(0, 64)));
                BigInteger Q_y = new BigInteger(1, StringToByteArray(ArdPubKeyStr.Substring(64, 64)));
                ECPoint Q = curve.CreatePoint(Q_x, Q_y);

                ECPublicKeyParameters ArdPubKey = new ECPublicKeyParameters(Q, ecDomain);

                // Extract the shared secret for decryption
                IBasicAgreement agreement = new Org.BouncyCastle.Crypto.Agreement.ECDHBasicAgreement();
                agreement.Init(appPrivateKey);
                sharedKey = agreement.CalculateAgreement(ArdPubKey);
                Debug.WriteLine("Shared KEY: " + ByteArrayToString(sharedKey.ToByteArrayUnsigned()));

                characteristic.ValueUpdated += GetEncryptedMessagePortion;
                await characteristic.StartUpdatesAsync();
                return true;
            }
            catch (DeviceConnectionException)
            {
                Debug.WriteLine("Could not connect to device");
                return false;               
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Disconnects from the device the app is currently paired with.
        /// </summary>
        /// <returns>Returns whether the disconnect was successful.</returns>
        public async Task<bool> DisconnectDevice()
        {
            try
            {
                await characteristic.StopUpdatesAsync();
                characteristic.ValueUpdated -= GetEncryptedMessagePortion;
            
                await adapter.DisconnectDeviceAsync(PairedDevice);
                PairedDevice = null;
                OnPropertyChanged("Devices");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Decrypts and notifies MainActivity with the interpreted message contents. 
        /// </summary>
        /// <param name="encrypted_message"></param>
        private void DecryptMessageAndNotify(string encrypted_message)
        {
            byte[] iv = StringToByteArray(encrypted_message.Substring(0, 32));
            byte[] msg = StringToByteArray(encrypted_message.Substring(32, 32));
            byte[] secret = sharedKey.ToByteArrayUnsigned();
            string secret_str = ByteArrayToString(secret);
            byte[] iv1 = new byte[16];
            byte[] msg1 = new byte[16];
            byte[] secret1 = new byte[32];

            System.Buffer.BlockCopy(iv, 0, iv1, 0, iv.Length);
            System.Buffer.BlockCopy(msg, 0, msg1, 0, msg1.Length);
            System.Buffer.BlockCopy(secret, 0, secret1, 0, secret.Length);

            using (MemoryStream ms = new MemoryStream())
            {
                using (System.Security.Cryptography.AesManaged cryptor = new System.Security.Cryptography.AesManaged())
                {
                    cryptor.Mode = System.Security.Cryptography.CipherMode.CBC;
                    cryptor.Padding = System.Security.Cryptography.PaddingMode.None;
                    cryptor.KeySize = 256;
                    cryptor.BlockSize = 128;

                    using (System.Security.Cryptography.CryptoStream cs = new System.Security.Cryptography.CryptoStream(ms, cryptor.CreateDecryptor(secret1, iv1), System.Security.Cryptography.CryptoStreamMode.Write))
                    {
                        cs.Write(msg1, 0, msg1.Length);
                    }

                    byte[] result = ms.ToArray();
                    Task.Run(() => InterperetGesture(result[0]));
                }
            }

            EncryptedMsg = "";
        }

        /// <summary>
        /// Interprets the byte to a gesture and notifies MainActivity of its arrival. 
        /// </summary>
        /// <param name="b">Byte to be interpreted.</param>
        private void InterperetGesture(byte b)
        {
            switch (b.ToString())
            {
                case "0":
                    Debug.WriteLine("UP");
                    var gesture_up = new GestureReceivedMessage("UP");
                    MessagingCenter.Send(gesture_up, "UP");
                    break;
                case "1":
                    Debug.WriteLine("DOWN");
                    var gesture_down = new GestureReceivedMessage("DOWN");
                    MessagingCenter.Send(gesture_down, "DOWN");
                    break;
                case "2":
                    Debug.WriteLine("LEFT");
                    var gesture_left = new GestureReceivedMessage("LEFT");
                    MessagingCenter.Send(gesture_left, "LEFT");
                    break;
                case "3":
                    Debug.WriteLine("RIGHT");
                    var gesture_right = new GestureReceivedMessage("RIGHT");
                    MessagingCenter.Send(gesture_right, "RIGHT");
                    break;
            }
        }

        /// <summary>
        /// Checks if the entire key and starting message have been received. 
        /// </summary>
        private void CheckIfKeyObtainComplete()
        {
            while (true)
            {
                if (ArdPubKeyStr.Length >= 136)
                    break;
            }
            return;
        }

        /// <summary>
        /// Stores current Bluetooth state for the phone.
        /// </summary>
        public string BluetoothState
        {
            // TODO: do we need this? not used in view 
            get { return bluetoothState; }
            set
            {
                if (bluetoothState != value)
                {
                    bluetoothState = value;
                    OnPropertyChanged("BluetoothState");
                }
            }
        }

        /// <summary>
        /// Gets the encrypted message from the Bluetooth sender.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void GetEncryptedMessagePortion(object sender, Plugin.BLE.Abstractions.EventArgs.CharacteristicUpdatedEventArgs e)
        {
            if (EncryptedMsg.Length < 64)
                EncryptedMsg += e.Characteristic.StringValue;

            if (EncryptedMsg.Length == 64)
                await Task.Run(() => DecryptMessageAndNotify(EncryptedMsg));
        }

        /// <summary>
        /// Get the public key from the sender.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GetPubKeyPortion(object sender, Plugin.BLE.Abstractions.EventArgs.CharacteristicUpdatedEventArgs e)
        {
            if (ArdPubKeyStr.Length < 136)
                ArdPubKeyStr += e.Characteristic.StringValue;
        }

        /// <summary>
        /// Converts the string of hex characters to a byte array.
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        private static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        /// <summary>
        /// Converts the byte array to a string of hex characters. 
        /// </summary>
        /// <param name="ba"></param>
        /// <returns></returns>
        private static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

    }

}

