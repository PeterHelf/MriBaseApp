using MriBase.App.Base.Bluetooth;
using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.ViewModels;
using MriBase.App.Base.Views;
using MriBase.Models.Enums;
using MriBase.Models.Models;
using MriBase.Models.Services.Interfaces;
using System;

namespace MriBase.App.Base.Services.Implementations
{
    public class TrainingPageSelectionService : ITrainingPageSelectionService
    {
        private readonly INavigationService navigationService;
        private readonly IOfflineChangesManager offlineChangesManager;
        private readonly IFeederService feederService;
        private readonly ILocalSaveService localSaveService;
        private readonly IAppDataService appDataService;
        private readonly IBluetoothGATTServer bluetoothGATTServer;

        public TrainingPageSelectionService(INavigationService navigationService, IOfflineChangesManager offlineChangesManager, IFeederService feederService, ILocalSaveService localSaveService, IAppDataService appDataService, IBluetoothGATTServer bluetoothGATTServer)
        {
            this.navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            this.offlineChangesManager = offlineChangesManager ?? throw new ArgumentNullException(nameof(offlineChangesManager));
            this.feederService = feederService ?? throw new ArgumentNullException(nameof(feederService));
            this.localSaveService = localSaveService ?? throw new ArgumentNullException(nameof(localSaveService));
            this.appDataService = appDataService ?? throw new ArgumentNullException(nameof(appDataService));
            this.bluetoothGATTServer = bluetoothGATTServer ?? throw new ArgumentNullException(nameof(bluetoothGATTServer));
        }

        public BaseTrainingPage GetTrainingPage(Training training, bool startWithBluetooth = false)
        {
            BaseTrainingPage contentPage;

            switch (training.TrainingType)
            {
                case TrainingType.RndPosTest:
                    contentPage = new RandomPositionTestPage(new RandomPositionTestViewModel(training, navigationService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer));
                    break;
                case TrainingType.TwoImgTest:
                    contentPage = new TwoImagesTestPage(new TwoImagesTestViewModel(training, navigationService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer));
                    break;
                case TrainingType.SeveralImgTest:
                    contentPage = new SeveralImagesTestPage(new SeveralImagesTestViewModel(training, navigationService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer));
                    break;
                case TrainingType.MatchingToSample:
                    contentPage = new MatchingToSampleTestPage(new MatchingToSampleTestViewModel(training, navigationService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer));
                    break;
                case TrainingType.GoNoGo:
                    contentPage = new GoNoGoTestPage(new GoNoGoTestViewModel(training, navigationService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer));
                    break;
                case TrainingType.SequentialLearning:
                    contentPage = new SequentialLearningTestPage(new SequentialLearningTestViewModel(training, navigationService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer));
                    break;
                case TrainingType.DeathRecognition:
                    contentPage = new DeathRecognitionTestPage(new DeathRecognitionTestViewModel(training, navigationService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer));
                    break;
                case TrainingType.DeathRecognitionTraining1:
                    contentPage = new DeathRecognitionTraining1Page(new DeathRecognitionTraining1ViewModel(training, navigationService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer));
                    break;
                case TrainingType.DeathRecognitionTraining2:
                    contentPage = new DeathRecognitionTraining2Page(new DeathRecognitionTraining2ViewModel(training, navigationService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer));
                    break;
                case TrainingType.SingleImageTraining:
                    contentPage = new SingleImageTrainingPage(new SingleImageTrainingViewModel(training, navigationService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer));
                    break;
                case TrainingType.EntireTouchscreenTraining:
                    contentPage = new EntireTouchscreenTrainingPage(new EntireTouchscreenTrainingViewModel(training, navigationService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer));
                    break;
                case TrainingType.SingleImageMultiplePositionsTraining:
                    contentPage = new SingleImageMultiplePositionsTrainingPage(new SingleImageMultiplePositionsTrainingViewModel(training, navigationService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            contentPage.ViewModel.BroadcastResultWithBluetooth = startWithBluetooth;

            return contentPage;
        }
    }
}
