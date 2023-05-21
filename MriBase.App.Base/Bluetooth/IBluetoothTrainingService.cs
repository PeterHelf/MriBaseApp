using Plugin.BluetoothLE.Server;

namespace MriBase.App.Base.Bluetooth
{
    public interface IBluetoothTrainingService
    {
        void StartTraining(IDevice device, int trainingId);
    }
}