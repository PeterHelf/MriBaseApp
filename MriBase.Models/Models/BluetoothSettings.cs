using System;
using System.Collections.Generic;

namespace MriBase.Models.Models
{
    [Serializable]
    public class BluetoothSettings
    {
        public List<string> FeederNames { get; }


        public List<Guid> FeederServiceUIDs { get; }
        public List<Guid> FeederReadCharacteristicIds { get; }
        public List<Guid> FeederWriteCharacteristicIds { get; }

        public BluetoothSettings()
        {
            this.FeederNames = new List<string>();
            this.FeederServiceUIDs = new List<Guid>();
            this.FeederReadCharacteristicIds = new List<Guid>();
            this.FeederWriteCharacteristicIds = new List<Guid>();

            FeederNames.Add("DogToy");
            FeederNames.Add("MLT-BT");

            FeederServiceUIDs.Add(Guid.Parse("0000ffe0-0000-1000-8000-00805f9b34fb"));
            FeederServiceUIDs.Add(Guid.Parse("6E400001-B5A3-F393-E0A9-E50E24DCCA9E"));

            FeederReadCharacteristicIds.Add(Guid.Parse("0000ffe1-0000-1000-8000-00805f9b34fb"));
            FeederReadCharacteristicIds.Add(Guid.Parse("6E400003-B5A3-F393-E0A9-E50E24DCCA9E"));

            FeederWriteCharacteristicIds.Add(Guid.Parse("0000ffe1-0000-1000-8000-00805f9b34fb"));
            FeederWriteCharacteristicIds.Add(Guid.Parse("6E400002-B5A3-F393-E0A9-E50E24DCCA9E"));
        }
    }
}
