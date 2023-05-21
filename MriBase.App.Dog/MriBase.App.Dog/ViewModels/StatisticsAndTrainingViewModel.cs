using MriBase.App.Base.Bluetooth;
using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.ViewModels;
using MriBase.Models.Resources;
using MriBase.Models.Services.Interfaces;

namespace MriBase.App.Dog.ViewModels
{
    public class StatisticsAndTrainingViewModel : StatisticsAndTrainingViewModelBase
    {
        public StatisticsAndTrainingViewModel(INavigationService navigationService, IFeederService feederService, IAppDataService appDataService, IImageRecourceService imageRecourceService) : base(navigationService, feederService, appDataService, imageRecourceService)
        {
        }

        public override string AnimalSelectionText => ResViewStatisticsAndTraining.DogSelection;
    }
}