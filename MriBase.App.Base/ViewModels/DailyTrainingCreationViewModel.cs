using MriBase.App.Base.Services.Interfaces;
using MriBase.Models.Enums;
using MriBase.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace MriBase.App.Base.ViewModels
{
    public class DailyTrainingCreationViewModel : BaseViewModel
    {
        private readonly INavigationService navigationService;
        private readonly IAppDataService appDataService;
        private readonly ILocalSaveService localSaveService;
        private bool anyTraining;
        public TimeSpan StartTime { get; set; }

        public int? MinDuration { get; set; }

        public int? MaxDuration { get; set; }

        public bool AnyTraining
        {
            get => this.anyTraining;
            set
            {
                this.anyTraining = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.TrainingSelectionAvailable));
            }
        }

        public bool TrainingSelectionAvailable => !AnyTraining;

        public TrainingType SelectedTrainingType { get; set; }

        public AnimalInformationViewModel SelectedAnimal { get; set; }

        public IEnumerable<TrainingType> AvailableTrainings => Enum.GetValues(typeof(TrainingType)).Cast<TrainingType>();

        public List<AnimalInformationViewModel> AvailableAnimals => this.appDataService.Animals.Select(a => new AnimalInformationViewModel(a)).ToList();

        public DailyTrainingCreationViewModel(INavigationService navigationService, ITimedTrainingsService timedTrainingsService, IAppDataService appDataService, ILocalSaveService localSaveService)
        {
            this.navigationService = navigationService;
            this.appDataService = appDataService;
            this.localSaveService = localSaveService;
            this.AddTrainingCommand = new Command(async () =>
            {
                if (this.MinDuration.HasValue && this.MaxDuration.HasValue && !(this.SelectedAnimal is null))
                {
                    var training = new TimedTraining(this.MinDuration.Value, this.MaxDuration.Value, this.StartTime, this.AnyTraining,
                    this.SelectedTrainingType, this.SelectedAnimal.AnimalInformation);
                    this.appDataService.LogedInUser.DailyTrainings.Add(training);
                    timedTrainingsService.StartTraining(training);
                    await this.localSaveService.SaveAnimals();
                    await this.navigationService.ReturnToLastPage();
                }
            });
        }

        public Command AddTrainingCommand { get; set; }
    }
}