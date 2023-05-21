using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.Views;
using MriBase.Models.Interfaces;
using MriBase.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace MriBase.App.Base.ViewModels
{
    public class MainPhoneViewModel : BaseViewModel
    {
        private readonly INavigationService navigationService;
        private readonly IAppDataService appDataService;

        public MainPhoneViewModel(INavigationService navigationService, ILocalSaveService localSaveService, IAppDataService appDataService)
        {
            this.navigationService = navigationService;
            this.appDataService = appDataService;
            this.appDataService.SelectedAnimalChanged += MainPhoneViewModelSelectedAnimalChanged;

            this.SelectAnimalCommand = new Command(async a =>
            {
                if (!(a is AnimalInformationViewModel animalInfo))
                {
                    return;
                }
                if (this.appDataService.SelectedAnimal != animalInfo.AnimalInformation)
                {
                    this.appDataService.SelectedAnimal = animalInfo.AnimalInformation;
                    OnPropertyChanged(nameof(AnimalImage));

                    var trainings = await this.appDataService.Trainings;

                    foreach (var stat in this.appDataService.SelectedAnimal.Statistics)
                    {
                        stat.Training = trainings.FirstOrDefault(t => t.Id == stat.TrainingId);
                    }

                    var newStatisticsTrainingIds = new List<int>();

                    foreach (var training in trainings.Where(t => this.appDataService.SelectedAnimal.Statistics.All(s => s.TrainingId != t.Id)))
                    {
                        var newStat = new MriBase.Models.Models.TrainingStatistic(training);

                        newStatisticsTrainingIds.Add(training.Id);
                        this.appDataService.SelectedAnimal.Statistics.Add(newStat);
                    }

                    this.appDataService.SelectedAnimal.Statistics.RemoveAll(s => s.Training is null);
                    await localSaveService.SaveAnimals();
                }
            });

            this.EditAnimalCommand = new Command(async a =>
            {
                if (!(a is AnimalInformationViewModel animalInfo))
                {
                    return;
                }

                await this.navigationService.NavigateToWithFactoryAsync<AnimalEditPageBase, IAnimalInformation>(animalInfo.AnimalInformation);
            });
        }

        private void MainPhoneViewModelSelectedAnimalChanged(object sender, EventArgs e)
        {
            this.OnPropertyChanged(nameof(AnimalImage));
        }

        public ImageSource AnimalImage => ImageSource.FromStream(() => new MemoryStream(this.appDataService.SelectedAnimal.Image));

        public Command SelectAnimalCommand { get; set; }
        public Command EditAnimalCommand { get; set; }

        public IEnumerable<AnimalInformationViewModel> Animals =>
            this.appDataService.Animals.Select(a => new AnimalInformationViewModel(a));
    }
}
