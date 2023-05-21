using System;
using System.Collections.Generic;
using System.Text;

namespace MriBase.App.Base.Services.Interfaces
{
    public interface IConfigService
    {
        bool BluetoothEnabled { get; }

        string ApiEndpoint { get; }

        string BleAppServiceUUID { get; }

        string BleAppCharacteristicUUID { get; }

        string AppName { get; }

        string CheckUpdateEndpoint { get; }
    }
}
