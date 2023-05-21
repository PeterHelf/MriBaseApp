using MriBase.Models.Models;
using Xamarin.Forms;

namespace MriBase.App.Base.Bluetooth
{
    public interface IBluetoothGATTServer
    {
        string CharacteristicValue { get; set; }
        Command Clear { get; }
        string Output { get; }
        string ServerText { get; set; }
        Command ToggleServer { get; }

        void BroadcastTrainingResult(TrainingSessionResult trainingResult);
        void BroadcastTrainingTrialResult(TrainingTrialResult trialResult);
        void StartServer();
    }
}