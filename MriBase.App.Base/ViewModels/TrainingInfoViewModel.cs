using MriBase.App.Base.Bluetooth;
using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.Views;
using MriBase.Models.Models;
using MriBase.Models.Resources;
using System.Linq;
using Xamarin.Forms;

namespace MriBase.App.Base.ViewModels
{
    public class TrainingInfoViewModel : BaseViewModel
    {
        private readonly IAppDataService appDataService;

        public TrainingViewModel Training { get; }

        public TrainingsStatisticViewModel Statistic { get; }

        //TODO: Set url for explanation videos
        public string ExplanationVideoUrl => string.Empty;

        public TrainingInfoViewModel(Training training, INavigationService navigationService, IBluetoothService bluetoothService, IAppDataService appDataService)
        {
            this.appDataService = appDataService;
            Training = new TrainingViewModel(training);
            this.Statistic = new TrainingsStatisticViewModel(this.appDataService.SelectedAnimal.Statistics.FirstOrDefault(s => s.Training == training));

            this.StartTrainingCommand = new Command(async () =>
            {
                this.IsBusy = true;
                this.BusyText = ResViewTrainings.StartingTraining;

                if (await bluetoothService.StartTrainingOnDevice(training.Id))
                {
                    await navigationService.NavigateToWithFactoryAsync<BluetoothTrainingPage, Training>(training);
                }

                this.IsBusy = false;
            });
        }

        public Command StartTrainingCommand { get; set; }
    }
}
