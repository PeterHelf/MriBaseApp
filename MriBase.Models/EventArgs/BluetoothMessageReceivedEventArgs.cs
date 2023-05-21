using MriBase.Models.Bluetooth;

namespace MriBase.Models.EventArgs
{
    public class BluetoothMessageReceivedEventArgs
    {
        public BluetoothMessage BluetoothMessage { get; }

        public BluetoothMessageReceivedEventArgs(byte[] byteArray)
        {
            this.BluetoothMessage = new BluetoothMessage(byteArray);
        }
    }
}
