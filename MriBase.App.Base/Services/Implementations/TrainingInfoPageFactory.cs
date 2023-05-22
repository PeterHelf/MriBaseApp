using MriBase.App.Base.Bluetooth;
using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.ViewModels;
using MriBase.App.Base.Views;
using MriBase.Models.Models;
using System;

namespace MriBase.App.Base.Services.Implementations
{
    public class TrainingInfoPageFactory : ITrainingInfoPageFactory
    {
        private readonly INavigationService navigationService;
        private readonly IBluetoothService bluetoothService;
        private readonly IAppDataService appDataService;
        private readonly ITrainingPageSelectionService trainingPageSelectionService;
        private readonly IConfigService configService;

        public TrainingInfoPageFactory(INavigationService navigationService, IBluetoothService bluetoothService, IAppDataService appDataService, ITrainingPageSelectionService trainingPageSelectionService, IConfigService configService)
        {
            this.navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            this.bluetoothService = bluetoothService ?? throw new ArgumentNullException(nameof(bluetoothService));
            this.appDataService = appDataService ?? throw new ArgumentNullException(nameof(appDataService));
            this.trainingPageSelectionService = trainingPageSelectionService ?? throw new ArgumentNullException(nameof(trainingPageSelectionService));
            this.configService = configService ?? throw new ArgumentNullException(nameof(configService));
        }

        public TrainingInfoPage CreateInstance(Training training)
        {
            return new TrainingInfoPage(new TrainingInfoViewModel(training, navigationService, bluetoothService, appDataService, trainingPageSelectionService, configService));
        }
    }
}
