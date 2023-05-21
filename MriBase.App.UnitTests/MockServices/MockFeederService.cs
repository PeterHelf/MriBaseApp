using MriBase.App.Base.Services.Interfaces;
using MriBase.Models.EventArgs;
using System;
using System.Threading.Tasks;

namespace MriBase.App.UnitTests.MockServices
{
    internal class MockFeederService : IFeederService
    {
        public bool FeederDeviceConnected => throw new NotImplementedException();

        public bool IsScanning => throw new NotImplementedException();

        public event EventHandler FeederScanStarted;
        public event EventHandler FeederScanStopped;
        public event EventHandler FeederConnected;
        public event EventHandler FeederDisconnected;
        public event EventHandler<BytesReceivedEventArgs> BytesReceived;

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
    }
}