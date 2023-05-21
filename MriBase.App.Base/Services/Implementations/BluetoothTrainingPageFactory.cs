using MriBase.App.Base.Bluetooth;
using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.ViewModels;
using MriBase.App.Base.Views;
using MriBase.Models.Models;

namespace MriBase.App.Base.Services.Implementations
{
    public class BluetoothTrainingPageFactory : IFactory<BluetoothTrainingPage, Training>, IBluetoothTrainingPageFactory
    {
        private readonly IBluetoothService bluetoothService;

        public BluetoothTrainingPageFactory(IBluetoothService bluetoothService)
        {
            this.bluetoothService = bluetoothService ?? throw new System.ArgumentNullException(nameof(bluetoothService));
        }

        public BluetoothTrainingPage CreateInstance(Training training)
        {
            return new BluetoothTrainingPage(new BluetoothTrainingViewModel(training, bluetoothService));
        }
    }
}
