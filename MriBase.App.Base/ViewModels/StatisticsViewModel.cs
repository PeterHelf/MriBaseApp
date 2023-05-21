using MriBase.App.Base.ExtensionMethods;
using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.Views;
using MriBase.Models.Models;
using MriBase.Models.Resources;
using MriBase.Models.Translation;
using Plugin.Connectivity;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace MriBase.App.Base.ViewModels
{
    public class StatisticsViewModel : BaseViewModel
    {
        private IEnumerable<TrainingStatistic> allStatistics;

        private string searchText;
        private readonly INavigationService navigationService;
        private readonly IAppDataService appDataService;

        public IEnumerable<TrainingsStatisticViewModel> Statistics => allStatistics.Where(s => s.Name.ToLower().Contains(this.SearchText.ToLower())).Select(s => new TrainingsStatisticViewModel(s));

        public Command ShowDetailsPageCommand { get; set; }

        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                OnPropertyChanged(nameof(this.Statistics));
            }
        }

        public StatisticsViewModel(INavigationService navigationService, IAppDataService appDataService)
        {
            this.navigationService = navigationService;
            this.appDataService = appDataService;
            this.allStatistics = this.appDataService.SelectedAnimal.Statistics;
            this.searchText = string.Empty;

            this.appDataService.SelectedAnimalChanged += StatisticsViewModelSelectedAnimalChanged;
        }

        private void StatisticsViewModelSelectedAnimalChanged(object sender, System.EventArgs e)
        {
            this.allStatistics = this.appDataService.SelectedAnimal.Statistics;
            this.OnPropertyChanged(nameof(this.Statistics));
        }
    }
}