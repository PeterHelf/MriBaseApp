using MriBase.App.Base.Services.Interfaces;
using MriBase.Models.EventArgs;
using System;
using System.IO.Ports;
using System.Threading.Tasks;

namespace MriBase.App.Base.Services.Implementations
{
    public class USBFeederService : IFeederService
    {
        private SerialPort feederPort;

        public bool FeederDeviceConnected => !(FeederPort is null);

        public bool IsScanning { get; private set; }

        public SerialPort FeederPort
        {
            get => this.feederPort;

            private set
            {
                if (this.feederPort == value)
                {
                    return;
                }

                if (value is null)
                {
                    this.feederPort.DataReceived -= this.FeederDataReceived;
                }

                this.feederPort = value;
                if (this.feederPort is null)
                {
                    this.FeederDisconnected?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    this.FeederConnected?.Invoke(this, EventArgs.Empty);
                    this.feederPort.DataReceived += this.FeederDataReceived;
                }
            }
        }

        private void FeederDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var port = sender as SerialPort;
            var buffer = new byte[port.BytesToRead];

            port.Read(buffer, 0, buffer.Length);

            this.BytesReceived(this, new BytesReceivedEventArgs(buffer));
        }

        public event EventHandler FeederScanStarted;
        public event EventHandler FeederScanStopped;
        public event EventHandler FeederConnected;
        public event EventHandler FeederDisconnected;
        public event EventHandler<BytesReceivedEventArgs> BytesReceived;

        public Task DisconnectFeeder()
        {
            this.feederPort?.Close();
            this.feederPort?.Dispose();
            this.feederPort = null;
            return Task.CompletedTask;
        }

        public Task FillFood()
        {
            return Task.CompletedTask;
        }

        public Task GiveOutFood()
        {
            if (this.FeederDeviceConnected)
            {
                this.feederPort.WriteLine("1");
            }

            return Task.CompletedTask;
        }

        public Task ScanForDevices()
        {
            this.IsScanning = true;
            this.FeederScanStarted?.Invoke(this, EventArgs.Empty);

            Task.Run(() =>
            {
                var ports = SerialPort.GetPortNames();

                if (ports.Length != 0)
                {
                    var port = new SerialPort(ports[0]);
                    port.Open();

                    this.FeederPort = port;
                }

                this.IsScanning = false;
                this.FeederScanStopped?.Invoke(this, EventArgs.Empty);
            });

            return Task.CompletedTask;
        }
    }
}
