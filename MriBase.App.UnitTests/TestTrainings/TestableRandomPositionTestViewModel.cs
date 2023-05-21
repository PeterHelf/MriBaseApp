using MriBase.App.Base.Bluetooth;
using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.ViewModels;
using MriBase.Models.Models;
using MriBase.Models.Services.Interfaces;
using System.Collections.Generic;

namespace MriBase.App.UnitTests.TestTrainings
{
    internal class TestableRandomPositionTestViewModel : RandomPositionTestViewModel
    {
        public TestableRandomPositionTestViewModel(Training training, INavigationService navigationService, IRestService restService, IOfflineChangesManager offlineChangesManager, IFeederService feederService, ILocalSaveService localSaveService, IAppDataService appDataService, IBluetoothGATTServer bluetoothGATTServer) : base(training, navigationService, restService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer)
        {
        }

        public new List<TrainingTrial> ActualTrials => base.ActualTrials;
    }
}
