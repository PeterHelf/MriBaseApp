using MriBase.App.Base.ExtensionMethods;
using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.Views;
using MriBase.Models.Models;
using MriBase.Models.Resources;
using MriBase.Models.Translation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace MriBase.App.Base.ViewModels
{
    public class TrainingSelectionViewModel : BaseViewModel
    {
        public TrainingSelectionViewModel(INavigationService navigationService, ITrainingPageSelectionService trainingPageSelectionService, IAppDataService appDataService)
        {
            this.appDataService = appDataService;
            this.trainings = this.appDataService.Trainings.Result;
            this.navigationService = navigationService;
            this.searchText = string.Empty;

            if (Device.Idiom == TargetIdiom.Phone)
            {
                ItemClickedCommand = new Command(t =>
                {
                    if (t is TrainingViewModel training)
                    {
                        this.navigationService.NavigateToWithFactoryAsync<TrainingInfoPage, Training>(training.Training);
                    }
                });
            }
            else
            {
                ItemClickedCommand = new Command(t =>
                {
                    var training = t as TrainingViewModel;

                    var trainingPage = trainingPageSelectionService.GetTrainingPage(training.Training);

                    this.navigationService.NavigateToAsync(trainingPage);
                });
            }

        }

        private readonly Training[] trainings;
        private string searchText;
        private readonly INavigationService navigationService;
        private readonly IAppDataService appDataService;

        public Command ItemClickedCommand { get; set; }

        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                OnPropertyChanged(nameof(this.TrainingVms));
            }
        }

        public IEnumerable<TrainingViewModel> TrainingVms => trainings.Where(t => t.Name.ToString().ToLower().Contains(this.SearchText.ToLower())).Select(t => new TrainingViewModel(t));
    }
}