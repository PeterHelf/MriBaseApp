using MriBase.App.Base.Services.Interfaces;
using MriBase.Models;
using MriBase.Models.EventArgs;
using MriBase.Models.Models;
using MriBase.Models.Resources;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MriBase.App.Base.Bluetooth
{
    /// <summary>This class represents a BLE manager.</summary>
    public class BluetoothService : IBluetoothService, IFeederService
    {
        private bool updatesStarted;

        /// <summary>Gets or sets the BLE property.</summary>
        /// <value>The BLE property.</value>
        public IBluetoothLE BLE { get; set; }

        /// <summary>Gets or sets the BLE adapter.</summary>
        /// <value>The BLE adapter.</value>
        public IAdapter BLEAdapter { get; set; }

        /// <summary>Gets the found devices.</summary>
        /// <value>The found devices.</value>
        public List<IDevice> FoundDevices { get; private set; }

        /// <summary>Gets the connected devices.</summary>
        /// <value>The connected devices.</value>
        public List<IDevice> ConnectedDevices { get; private set; }

        public List<IDevice> FoundDisplayDevices { get; set; }
        public IDevice ConnectedDisplayDevice { get; set; }

        public BluetoothSettings BluetoothSettings { get; set; }

        public bool DisplayDeviceConnected => ConnectedDisplayDevice != null && BLEAdapter.ConnectedDevices.Contains(ConnectedDisplayDevice);

        public List<IDevice> FoundFeederDevices { get; set; }
        public bool FeederDeviceConnected => ConnectedFeederDevice != null && BLEAdapter.ConnectedDevices.Contains(ConnectedFeederDevice);
        public IDevice ConnectedFeederDevice { get; private set; }

        public bool IsScanning => BLEAdapter.IsScanning;

        public event EventHandler<BytesReceivedEventArgs> BytesReceived;
        public event EventHandler<BluetoothMessageReceivedEventArgs> BluetoothMessageReceived;
        public event EventHandler FeederScanStarted;
        public event EventHandler FeederConnected;
        public event EventHandler FeederScanStopped;
        public event EventHandler FeederDisconnected;

        private IService feederService;
        private ICharacteristic feederWriteCharacteristic;
        private ICharacteristic feederReadCharacteristic;
        private readonly IConfigService configService;

        /// <summary>Initializes a new instance of the <see cref="BluetoothService" /> class.</summary>
        public BluetoothService(IAppDataService appDataService, IConfigService configService)
        {
            BluetoothSettings = appDataService.UserSettings.BluetoothSettings;

            try
            {
                BLE = CrossBluetoothLE.Current;
                BLEAdapter = CrossBluetoothLE.Current.Adapter;

                BLEAdapter.ScanTimeout = 5000; // Search duration is 5000 ms
                BLEAdapter.DeviceDiscovered += DeviceDiscovered;
                BLEAdapter.DeviceConnected += (sender, e) => { this.FeederConnected?.Invoke(this, EventArgs.Empty); };
                BLEAdapter.DeviceDisconnected += (sender, e) => { this.FeederDisconnected?.Invoke(this, EventArgs.Empty); };
                BLEAdapter.DeviceConnectionLost += (sender, e) => { this.FeederDisconnected?.Invoke(this, EventArgs.Empty); };
                BLEAdapter.ScanTimeoutElapsed += (sender, e) => { this.FeederScanStopped?.Invoke(this, EventArgs.Empty); };
            }
            catch (Exception)
            {
            }

            this.FoundDevices = new List<IDevice>();
            this.ConnectedDevices = new List<IDevice>();
            this.FoundDisplayDevices = new List<IDevice>();
            this.FoundFeederDevices = new List<IDevice>();
            this.configService = configService;
        }

        /// <summary>Clears the current list of found devices and searches for new devices.</summary>
        public async Task ScanForDevices()
        {
            if (this.BLEAdapter.IsScanning)
            {
                return;
            }

            FoundDevices.Clear();
            var scanTask = BLEAdapter.StartScanningForDevicesAsync();
            this.FeederScanStarted?.Invoke(this, EventArgs.Empty);

            await scanTask;
        }

        /// <summary>Adds a found device to the list of found devices, if the device has a name.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DeviceDiscovered(object sender, DeviceEventArgs e)
        {
            this.FoundDevices.Add(e.Device);

            if (Device.Idiom == TargetIdiom.Phone)
            {
                //HACK
                if (e.Device.AdvertisementRecords.Any(a => a.Type == AdvertisementRecordType.UuidsComplete128Bit && a.Data.OrderBy(x => x).SequenceEqual(Guid.Parse(this.configService.BleAppServiceUUID).ToByteArray().OrderBy(x => x))))
                {
                    this.FoundDisplayDevices.Add(e.Device);

                    var connect = false;
                    await Device.InvokeOnMainThreadAsync(async () => connect = await Application.Current.MainPage.DisplayAlert(ResViewBluetooth.DeviceFound, ResViewBluetooth.ConnectToNearbyDevice, ResViewBasics.Yes, ResViewBasics.No));

                    if (connect)
                    {
                        if (this.DisplayDeviceConnected)
                        {
                            await this.DisconnectDevice(ConnectedDisplayDevice);
                        }

                        await this.ConnectToDevice(e.Device);

                        this.ConnectedDisplayDevice = e.Device;
                    }
                }
            }
            else
            {
                if (!(e.Device.Name is null) && BluetoothSettings.FeederNames.Any(n => e.Device.Name.StartsWith(n)))
                {
                    //if (!await this.CheckFeederBluetoothService(e.Device))
                    //{
                    //    await this.DisconnectDevice(e.Device);
                    //    return;
                    //}                    

                    if (this.FeederDeviceConnected)
                    {
                        await this.DisconnectDevice(ConnectedFeederDevice);
                    }

                    this.FoundFeederDevices.Add(e.Device);

                    await this.ConnectToDevice(e.Device);
                    this.ConnectedFeederDevice = e.Device;
                    this.FeederConnected?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>Connects to a specified device.</summary>
        /// <param name="device">The specified device.</param>
        /// <exception cref="ArgumentNullException">Is thrown if the specified instance is NULL.</exception>
        public async Task ConnectToDevice(IDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device), "The specified instance must not be NULL!");
            }

            if (device.State == DeviceState.Connected)
            {
                return;
            }

            await BLEAdapter.ConnectToDeviceAsync(device, new ConnectParameters(forceBleTransport: true));
        }

        /// <summary>Disconnects the specified device.</summary>
        /// <param name="device">The specified device.</param>
        /// <exception cref="ArgumentNullException">Is thrown if the specified instance is NULL.</exception>
        public async Task DisconnectDevice(IDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device), "The specified instance must not be NULL!");
            }

            await BLEAdapter.DisconnectDeviceAsync(device);
        }

        public async Task DisconnectFeeder()
        {
            var device = this.ConnectedFeederDevice;
            this.ConnectedFeederDevice = null;
            this.updatesStarted = false;

            this.feederService = null;
            this.feederReadCharacteristic = null;
            this.feederWriteCharacteristic = null;

            if (device is null)
            {
                return;
            }

            await BLEAdapter.DisconnectDeviceAsync(device);
        }

        public async Task<bool> StartTrainingOnDevice(int trainingId)
        {
            if (this.ConnectedDisplayDevice is null)
            {
                var search = false;

                await Device.InvokeOnMainThreadAsync(async () => search = await Application.Current.MainPage.DisplayAlert(ResViewBluetooth.NoDeviceConnected, ResViewBluetooth.SearchForDevice, ResViewBasics.Yes, ResViewBasics.No));

                if (search)
                {
                    await this.ScanForDevices();

                    if (DisplayDeviceConnected)
                    {
                        var startTraining = false;
                        await Device.InvokeOnMainThreadAsync(async () => startTraining = await Application.Current.MainPage.DisplayAlert(ResViewBluetooth.DeviceConnected, ResViewBluetooth.StartTrainingNow, ResViewBasics.Yes, ResViewBasics.No));

                        if (startTraining)
                        {
                            return await this.WriteToDevice(this.ConnectedDisplayDevice, this.configService.BleAppServiceUUID, this.configService.BleAppCharacteristicUUID, BitConverter.GetBytes(trainingId));
                        }
                    }
                    else
                    {
                        await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayAlert(ResViewBluetooth.NoDeviceFound, ResViewBluetooth.EnsureBluetoothDeviceNearby, ResViewBasics.Ok));
                    }
                }

                return false;
            }

            return await this.WriteToDevice(this.ConnectedDisplayDevice, this.configService.BleAppServiceUUID, this.configService.BleAppCharacteristicUUID, BitConverter.GetBytes(trainingId));
        }

        /// <summary>Writes to the connected device.</summary>
        /// <param name="device">The device.</param>
        /// <param name="command">The command.</param>
        /// <exception cref="ArgumentNullException">Is thrown if the specified instance is NULL.</exception>
        /// <exception cref="ArgumentNullException">Is thrown if the specified array is NULL.</exception>
        public async Task<bool> WriteToDevice(IDevice device, string serviceUUID, string characteristicUUID, byte[] command)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device), "The specified instance must not be NULL!");
            }

            if (command == null)
            {
                throw new ArgumentNullException(nameof(command), "The specified array must not be NULL!");
            }

            var service = await device.GetServiceAsync(Guid.Parse(serviceUUID));

            if (service != null)
            {
                var characteristic = await service.GetCharacteristicAsync(Guid.Parse(characteristicUUID));

                if (!this.updatesStarted)
                {
                    characteristic.ValueUpdated -= CharacteristicValueUpdated;
                    characteristic.ValueUpdated += CharacteristicValueUpdated;
                    this.updatesStarted = true;
                    try
                    {
                        await characteristic.StartUpdatesAsync();
                    }
                    catch (Exception)
                    {
                    }
                }

                return await characteristic?.WriteAsync(command);
            }

            return false;
        }

        private void CharacteristicValueUpdated(object sender, CharacteristicUpdatedEventArgs e)
        {
            var bytes = e.Characteristic.Value;

            this.OnBluetoothMessageReceived(bytes);
        }

        public async Task FillFood()
        {
            if (!this.FeederDeviceConnected)
            {
                return;
            }

            await WriteToFeeder(this.ConnectedFeederDevice, UTF8Encoding.UTF8.GetBytes("3"));
        }

        public async Task GiveOutFood()
        {
            if (!this.FeederDeviceConnected)
            {
                return;
            }

            await WriteToFeeder(this.ConnectedFeederDevice, UTF8Encoding.UTF8.GetBytes("2"));
        }

        /// <summary>
        /// Writes a specified byte array to the connected HM-10 adapter and it's corresponding Arduino.
        /// This method is HM-10 specific. A writing method for other devices is currently in developement.
        /// </summary>
        /// <param name="device">The connected HM-10 adapter.</param>
        /// <param name="command">The byte array.</param>
        /// <exception cref="ArgumentNullException">Is thrown if the specified instance is NULL.</exception>
        public async Task WriteToFeeder(IDevice device, byte[] command)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device), "The specified instance must not be NULL!");
            }

            if (device.State != DeviceState.Connected)
            {
                await this.DisconnectFeeder();
                return;
                //throw new DeviceConnectionException(device.Id, device.Name, "The device is not connected and cant therefore execute a command");
            }

            if (this.feederService is null)
            {
                IService service = null;

                foreach (var guid in BluetoothSettings.FeederServiceUIDs)
                {
                    service = await device.GetServiceAsync(guid);
                    if (!(service is null))
                    {
                        break;
                    }
                }

                if (service == null)
                    return;

                this.feederService = service;
            }

            if (this.feederWriteCharacteristic is null)
            {
                // Gets the write characteristic.
                ICharacteristic writeCharacteristic = null;

                foreach (var guid in BluetoothSettings.FeederWriteCharacteristicIds)
                {
                    writeCharacteristic = (await this.feederService.GetCharacteristicAsync(guid));
                    if (!(writeCharacteristic is null))
                    {
                        break;
                    }
                }

                if (writeCharacteristic == null)
                    return;

                this.feederWriteCharacteristic = writeCharacteristic;
            }

            if (this.feederReadCharacteristic is null)
            {

                // Gets the read characteristic.
                ICharacteristic readCharacteristic = null;

                foreach (var guid in BluetoothSettings.FeederReadCharacteristicIds)
                {
                    readCharacteristic = (await this.feederService.GetCharacteristicAsync(guid));
                    if (!(readCharacteristic is null))
                    {
                        break;
                    }
                }

                if (readCharacteristic == null)
                    return;

                this.feederReadCharacteristic = readCharacteristic;
            }

            if (!this.updatesStarted)
            {
                this.feederReadCharacteristic.ValueUpdated -= CharacteristicValueUpdated;
                this.feederReadCharacteristic.ValueUpdated += CharacteristicValueUpdated;
                try
                {
                    await this.feederReadCharacteristic.StartUpdatesAsync();
                    this.updatesStarted = true;
                }
                catch (Exception)
                {
                }
            }

            // Writes the specified byte array to the custom characteristic, if that characteristic is writeable. 
            if (this.feederWriteCharacteristic.CanWrite)
            {
                var test = await this.feederWriteCharacteristic.WriteAsync(command);
            }
        }

        private async Task<bool> CheckFeederBluetoothService(IDevice device)
        {
            IService service = null;

            foreach (var guid in BluetoothSettings.FeederServiceUIDs)
            {
                service = await device.GetServiceAsync(guid);
                if (!(service is null))
                {
                    break;
                }
            }

            if (service == null)
                return false;

            // Gets the write characteristic.
            ICharacteristic writeCharacteristic = null;

            foreach (var guid in BluetoothSettings.FeederWriteCharacteristicIds)
            {
                writeCharacteristic = (await service.GetCharacteristicAsync(guid));
                if (!(writeCharacteristic is null))
                {
                    break;
                }
            }

            if (writeCharacteristic == null)
                return false;

            // Gets the read characteristic.
            ICharacteristic readCharacteristic = null;

            foreach (var guid in BluetoothSettings.FeederReadCharacteristicIds)
            {
                readCharacteristic = (await service.GetCharacteristicAsync(guid));
                if (!(readCharacteristic is null))
                {
                    break;
                }
            }

            if (readCharacteristic == null)
                return false;

            if (writeCharacteristic.CanWrite && readCharacteristic.CanRead && readCharacteristic.CanUpdate)
            {
                return true;
            }

            return false;
        }

        protected void OnBluetoothMessageReceived(byte[] byteArray)
        {
            BluetoothMessageReceived?.Invoke(this, new BluetoothMessageReceivedEventArgs(byteArray));
            BytesReceived?.Invoke(this, new BytesReceivedEventArgs(byteArray));
        }
    }
}
