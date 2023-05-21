using MriBase.Models.Bluetooth;
using MriBase.Models.EventArgs;
using System;
using System.Threading.Tasks;

namespace MriBase.App.Base.Services.Interfaces
{
    public interface IFeederService
    {
        bool FeederDeviceConnected { get; }
        bool IsScanning { get; }

        event EventHandler FeederScanStarted;
        event EventHandler FeederScanStopped;
        event EventHandler FeederConnected;
        event EventHandler FeederDisconnected;
        event EventHandler<BytesReceivedEventArgs> BytesReceived;

        Task DisconnectFeeder();
        Task FillFood();
        Task GiveOutFood();
        Task ScanForDevices();
    }
}
