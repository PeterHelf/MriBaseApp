using MriBase.App.Base.Bluetooth;
using MriBase.Models.Bluetooth;
using MriBase.Models.EventArgs;
using MriBase.Models.Models;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MriBase.App.UnitTests.MockServices
{
    internal class MockBluetoothService : IBluetoothService
    {
        public IBluetoothLE BLE { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IAdapter BLEAdapter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public BluetoothSettings BluetoothSettings { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public List<IDevice> ConnectedDevices => throw new NotImplementedException();

        public IDevice ConnectedDisplayDevice { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IDevice ConnectedFeederDevice => throw new NotImplementedException();

        public bool DisplayDeviceConnected => throw new NotImplementedException();

        public bool FeederDeviceConnected => false;

        public List<IDevice> FoundDevices => throw new NotImplementedException();

        public List<IDevice> FoundDisplayDevices { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<IDevice> FoundFeederDevices { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool IsScanning => throw new NotImplementedException();

        public event EventHandler FeederScanStopped;
        public event EventHandler FeederDisconnected;

        public event EventHandler<BytesReceivedEventArgs> BytesReceived { add { } remove { } }
        public event EventHandler<BluetoothMessageReceivedEventArgs> BluetoothMessageReceived { add { } remove { } }
        public event EventHandler FeederScanStarted { add { } remove { } }
        public event EventHandler FeederConnected { add { } remove { } }

        public Task ConnectToDevice(IDevice device)
        {
            throw new NotImplementedException();
        }

        public Task DisconnectDevice(IDevice device)
        {
            throw new NotImplementedException();
        }

        public Task DisconnectFeeder()
        {
            throw new NotImplementedException();
        }

        public Task FillFood()
        {
            throw new NotImplementedException();
        }

        public Task GiveOutFood()
        {
            throw new NotImplementedException();
        }

        public Task ScanForDevices()
        {
            throw new NotImplementedException();
        }

        public Task<bool> StartTrainingOnDevice(int trainingId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> WriteToDevice(IDevice device, string serviceUUID, string characteristicUUID, byte[] command)
        {
            throw new NotImplementedException();
        }

        public Task WriteToFeeder(IDevice device, byte[] command)
        {
            throw new NotImplementedException();
        }
    }
}