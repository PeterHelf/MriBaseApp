using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.ViewModels;
using MriBase.App.Base.Views;
using MriBase.Models.Models;
using MriBase.Models.Services.Interfaces;
using System;

namespace MriBase.App.Base.Services.Implementations
{
    public class TrainingStatisticDetailsPageFactory : ITrainingStatisticDetailsPageFactory
    {
        private readonly IAppDataService appDataService;
        private readonly IImageRecourceService imageRecourceService;

        public TrainingStatisticDetailsPageFactory(IAppDataService appDataService, IImageRecourceService imageRecourceService)
        {
            this.appDataService = appDataService ?? throw new ArgumentNullException(nameof(appDataService));
            this.imageRecourceService = imageRecourceService;
        }

        public TrainingStatisticDetailsPage CreateInstance(TrainingStatistic statistic)
        {
            return new TrainingStatisticDetailsPage(new TrainingStatisticDetailsViewModel(statistic, appDataService, imageRecourceService));
        }
    }
}
