using System;

namespace MriBase.Models.Bluetooth
{
    public class BluetoothMessage
    {
        public bool IsSessionEnd { get; }

        public bool TrialCorrect { get; }

        public TimeSpan Duration { get; }

        public BluetoothMessage(byte[] bluetoothMessage)
        {
            this.IsSessionEnd = bluetoothMessage[0] == 1;
            this.TrialCorrect = bluetoothMessage[1] == 1;

            this.Duration = TimeSpan.FromTicks(BitConverter.ToInt64(bluetoothMessage, 2));
        }
    }
}
