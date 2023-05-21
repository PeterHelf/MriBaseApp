using MriBase.App.Base.Services.Interfaces;
using MriBase.Models.EventArgs;
using MriBase.Models.Models;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MriBase.App.Base.Bluetooth
{
    public interface IBluetoothService
    {
        IBluetoothLE BLE { get; set; }
        IAdapter BLEAdapter { get; set; }
        BluetoothSettings BluetoothSettings { get; set; }
        List<IDevice> ConnectedDevices { get; }
        IDevice ConnectedDisplayDevice { get; set; }
        IDevice ConnectedFeederDevice { get; }
        bool DisplayDeviceConnected { get; }
        List<IDevice> FoundDevices { get; }
        List<IDevice> FoundDisplayDevices { get; set; }
        List<IDevice> FoundFeederDevices { get; set; }

        event EventHandler<BluetoothMessageReceivedEventArgs> BluetoothMessageReceived;

        Task ConnectToDevice(IDevice device);
        Task DisconnectDevice(IDevice device);
        Task<bool> StartTrainingOnDevice(int trainingId);
        Task<bool> WriteToDevice(IDevice device, string serviceUUID, string characteristicUUID, byte[] command);
        Task WriteToFeeder(IDevice device, byte[] command);
    }
}