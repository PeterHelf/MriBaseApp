using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.Views;
using MriBase.Models.Services.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace MriBase.App.Base.ViewModels
{
    public class DailyTrainingsViewModel : BaseViewModel
    {
        private readonly INavigationService navigationService;
        private readonly IAppDataService appDataService;
        private readonly ILocalSaveService localSaveService;
        private readonly IImageRecourceService imageRecourceService;

        public IEnumerable<TimedTrainingsViewModel> ActiveDailyTrainings => this.appDataService.LogedInUser.DailyTrainings.Select(t => new TimedTrainingsViewModel(t));

        public ImageSource InfoIcon => ImageSource.FromStream(() => new MemoryStream(imageRecourceService.GetImage("infoIcon.png")));

        public DailyTrainingsViewModel(INavigationService navigationService, IAppDataService appDataService, ILocalSaveService localSaveService, IImageRecourceService imageRecourceService)
        {
            this.navigationService = navigationService;
            this.appDataService = appDataService;
            this.localSaveService = localSaveService;
            this.imageRecourceService = imageRecourceService;
            this.CreateNewTrainingCommand = new Command(() => this.navigationService.NavigateToAsync<DailyTrainingCreationPage>());
            this.DeleteTrainingCommand = new Command(async t =>
            {
                var training = t as TimedTrainingsViewModel;

                training?.TimedTraining.StopTimer();
                this.appDataService.LogedInUser.DailyTrainings.Remove(training.TimedTraining);
                this.OnPropertyChanged(nameof(this.ActiveDailyTrainings));
                await this.localSaveService.SaveAnimals();
            });
        }

        public Command DeleteTrainingCommand { get; set; }

        public Command CreateNewTrainingCommand { get; set; }

        public void Refresh()
        {
            OnPropertyChanged(nameof(this.ActiveDailyTrainings));
        }
    }
}