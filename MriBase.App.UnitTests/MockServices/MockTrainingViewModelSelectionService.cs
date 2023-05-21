using MriBase.App.Base.Bluetooth;
using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.ViewModels;
using MriBase.App.UnitTests.TestTrainings;
using MriBase.Models.Enums;
using MriBase.Models.Models;
using MriBase.Models.Services.Interfaces;
using System;

namespace MriBase.App.UnitTests.MockServices
{
    internal class MockTrainingViewModelSelectionService
    {
        private readonly INavigationService navigationService;
        private readonly IRestService restService;
        private readonly IOfflineChangesManager offlineChangesManager;
        private readonly IFeederService feederService;
        private readonly ILocalSaveService localSaveService;
        private readonly IAppDataService appDataService;
        private readonly IBluetoothGATTServer bluetoothGATTServer;

        public MockTrainingViewModelSelectionService(IRestService restService, IFeederService feederService, ILocalSaveService localSaveService, IAppDataService appDataService)
        {
            this.restService = restService;
            this.feederService = feederService;
            this.localSaveService = localSaveService;
            this.appDataService = appDataService;
            this.bluetoothGATTServer = null;
            this.offlineChangesManager = null;
            this.navigationService = null;
        }

        public BaseTrainingViewModel GetTrainingViewModel(Training training, bool startWithBluetooth = false)
        {
            BaseTrainingViewModel viewModel = training.TrainingType switch
            {
                TrainingType.RndPosTest => new TestableRandomPositionTestViewModel(training, navigationService, restService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer),
                TrainingType.TwoImgTest => new TestableTwoImagesTestViewModel(training, navigationService, restService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer),
                TrainingType.SeveralImgTest => new TestableSeveralImagesTestViewModel(training, navigationService, restService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer),
                TrainingType.MatchingToSample => new TestableMatchingToSampleTestViewModel(training, navigationService, restService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer),
                TrainingType.GoNoGo => new TestableGoNoGoTestViewModel(training, navigationService, restService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer),
                TrainingType.SequentialLearning => new TestableSequentialLearningTestViewModel(training, navigationService, restService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer),
                TrainingType.DeathRecognition => new DeathRecognitionTestViewModel(training, navigationService, restService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer),
                TrainingType.DeathRecognitionTraining1 => new DeathRecognitionTraining1ViewModel(training, navigationService, restService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer),
                TrainingType.DeathRecognitionTraining2 => new DeathRecognitionTraining2ViewModel(training, navigationService, restService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer),
                TrainingType.SingleImageTraining => new SingleImageTrainingViewModel(training, navigationService, restService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer),
                _ => throw new ArgumentOutOfRangeException(nameof(training)),
            };
            viewModel.BroadcastResultWithBluetooth = startWithBluetooth;

            return viewModel;
        }
    }
}