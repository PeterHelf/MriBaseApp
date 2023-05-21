using MriBase.App.Base.Services.Interfaces;
using MriBase.Models;
using MriBase.Models.Models;
using Plugin.BluetoothLE;
using Plugin.BluetoothLE.Server;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using IGattCharacteristic = Plugin.BluetoothLE.Server.IGattCharacteristic;

namespace MriBase.App.Base.Bluetooth
{
    public class BluetoothGATTServer : IBluetoothGATTServer
    {
        private readonly IAdapter adapter;
        private IGattServer server;
        private IGattCharacteristic mainCharacteristic;
        private readonly IBluetoothTrainingService bluetoothTrainingService;
        private readonly IConfigService configService;

        public BluetoothGATTServer(IBluetoothTrainingService bluetoothTrainingService, IConfigService configService)
        {
            this.adapter = CrossBleAdapter.Current;

            this.Clear = new Command(() => this.Output = String.Empty);
            this.bluetoothTrainingService = bluetoothTrainingService;
            this.configService = configService;
        }

        public string ServerText { get; set; } = "Start Server";

        public string CharacteristicValue { get; set; }
        public string Output { get; private set; }
        public Command ToggleServer { get; }
        public Command Clear { get; }

        public async void StartServer()
        {
            if (this.adapter.Status != AdapterStatus.PoweredOn)
            {
                //this.dialogs.Alert("Could not start GATT Server.  Adapter Status: " + this.adapter.Status);
                return;
            }

            if (!this.adapter.Features.HasFlag(AdapterFeatures.ServerGatt))
            {
                //this.dialogs.Alert("GATT Server is not supported on this platform configuration");
                return;
            }

            if (this.server == null)
            {
                await this.BuildServer();
                try
                {
                    this.adapter.Advertiser.Start(new AdvertisementData
                    {
                        //AndroidUseDeviceName = true,
                        LocalName = "MriBase.App",
                        ServiceUuids = new List<Guid>(new[] { Guid.Parse(this.configService.BleAppServiceUUID) })
                    });
                }
                catch (Exception)
                {

                }
            }
            else
            {
                this.ServerText = "Start Server";
                this.adapter.Advertiser.Stop();
                this.OnEvent("GATT Server Stopped");
                this.server.Dispose();
                this.server = null;
            }
        }

        private async Task BuildServer()
        {
            try
            {
                this.OnEvent("GATT Server Starting");
                this.server = await this.adapter.CreateGattServer();

                var service = this.server.CreateService(Guid.Parse(this.configService.BleAppServiceUUID), true);
                this.BuildCharacteristics(service, Guid.Parse(this.configService.BleAppCharacteristicUUID));
                this.server.AddService(service);
                this.ServerText = "Stop Server";

                this.server
                    .WhenAnyCharacteristicSubscriptionChanged()
                    .Subscribe(x =>
                        this.OnEvent($"[WhenAnyCharacteristicSubscriptionChanged] UUID: {x.Characteristic.Uuid} - Device: {x.Device.Uuid} - Subscription: {x.IsSubscribing}")
                    );

                this.OnEvent("GATT Server Started");
            }
            catch (Exception)
            {
                //this.dialogs.Alert("Error building gatt server - " + ex);
            }
        }

        private void BuildCharacteristics(Plugin.BluetoothLE.Server.IGattService service, Guid characteristicId)
        {
            var characteristic = service.AddCharacteristic(
                characteristicId,
                CharacteristicProperties.Read | CharacteristicProperties.Notify | CharacteristicProperties.Write | CharacteristicProperties.WriteNoResponse | CharacteristicProperties.Broadcast,
                GattPermissions.Read | GattPermissions.Write
            );

            characteristic
                .WhenDeviceSubscriptionChanged()
                .Subscribe(e =>
                {
                    var @event = e.IsSubscribed ? "Subscribed" : "Unsubcribed";
                    this.OnEvent($"Device {e.Device.Uuid} {@event}");
                    this.OnEvent($"Charcteristic Subcribers: {characteristic.SubscribedDevices.Count}");
                });

            characteristic.WhenWriteReceived().Subscribe(x =>
            {
                var trainingId = BitConverter.ToInt32(x.Value, 0);

                this.bluetoothTrainingService.StartTraining(x.Device, trainingId);
                this.OnEvent($"Training with Id {trainingId} started");
            });

            this.mainCharacteristic = characteristic;
        }

        public void BroadcastTrainingTrialResult(TrainingTrialResult trialResult)
        {
            var trialCorrect = trialResult.IsCorrect ? (byte)1 : (byte)0;

            var duration = trialResult.EndTime.Ticks - trialResult.StartTime.Ticks;

            var array = new[] { (byte)0, trialCorrect };
            var durationBytes = BitConverter.GetBytes(duration);

            var outputArray = new byte[array.Length + durationBytes.Length];
            array.CopyTo(outputArray, 0);
            durationBytes.CopyTo(outputArray, array.Length);

            this.Broadcast(outputArray);
        }

        public void BroadcastTrainingResult(TrainingSessionResult trainingResult)
        {
            var duration = trainingResult.SessionEndTime.Ticks - trainingResult.SessionBegin.Ticks;

            var array = new[] { (byte)1, (byte)0 };
            var durationBytes = BitConverter.GetBytes(duration);

            var outputArray = new byte[array.Length + durationBytes.Length];
            array.CopyTo(outputArray, 0);
            durationBytes.CopyTo(outputArray, array.Length);

            this.Broadcast(outputArray);
        }

        private void Broadcast(byte[] byteArray)
        {
            mainCharacteristic?.Broadcast(byteArray);
        }

        private void OnEvent(string msg)
        {
            //TODO: Logging
        }
    }
}
