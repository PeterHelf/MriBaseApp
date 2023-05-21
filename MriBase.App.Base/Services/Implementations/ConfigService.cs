using Microsoft.Extensions.Configuration;
using MriBase.App.Base.Services.Interfaces;
using MriBase.Models;
using System.IO;
using System.Text.Json;
using Xamarin.Forms;

namespace MriBase.App.Base.Services.Implementations
{
    public class ConfigService : IConfigService
    {
        private readonly IConfiguration config;

        public ConfigService(IConfiguration config)
        {
            this.config = config;
        }

        public bool BluetoothEnabled => bool.Parse(config.GetSection("Bluetooth")["Enabled"]);

        public string ApiEndpoint => config.GetSection("Endpoints")["Api"];

        public string BleAppServiceUUID => config.GetSection("Bluetooth")["BleAppServiceUUID"];

        public string BleAppCharacteristicUUID => config.GetSection("Bluetooth")["BleAppCharacteristicUUID"];

        public string AppName => config["AppName"];

        public string CheckUpdateEndpoint => config.GetSection("Endpoints")["CheckUpdateEndpoint"];
    }
}
