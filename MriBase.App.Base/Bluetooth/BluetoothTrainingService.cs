using MriBase.App.Base.Services.Interfaces;
using Plugin.BluetoothLE.Server;
using System.Linq;
using Xamarin.Forms;

namespace MriBase.App.Base.Bluetooth
{
    public class BluetoothTrainingService : IBluetoothTrainingService
    {
        private readonly IAppDataService appDataService;
        private readonly INavigationService navigationService;
        private readonly IContainer container;

        public BluetoothTrainingService(IAppDataService appDataService, INavigationService navigationService, IContainer container)
        {
            this.appDataService = appDataService;
            this.navigationService = navigationService;
            this.container = container;
        }

        public async void StartTraining(IDevice device, int trainingId)
        {
            var trainings = this.appDataService.Trainings.Result;

            var training = trainings.FirstOrDefault(t => t.Id == trainingId);

            if (training != null)
            {
                // HACK?
                var trainingPageSelectionService = this.container.Resolve<ITrainingPageSelectionService>();
                await Device.InvokeOnMainThreadAsync(async () =>
                {
                    await navigationService.NavigateToAsync(trainingPageSelectionService.GetTrainingPage(training, true));
                });
            }
        }
    }
}
