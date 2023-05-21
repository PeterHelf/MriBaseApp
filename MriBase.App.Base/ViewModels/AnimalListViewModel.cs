using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.Views;
using MriBase.Models.Interfaces;
using MriBase.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace MriBase.App.Base.ViewModels
{
    public class AnimalListViewModel : BaseViewModel
    {
        private readonly INavigationService navigationService;
        private readonly IAppDataService appDataService;
        private readonly ILocalSaveService localSaveService;
        private IEnumerable<AnimalInformationViewModel> animals;

        public AnimalListViewModel(INavigationService navigationService, IAppDataService appDataService, ILocalSaveService localSaveService)
        {
            this.navigationService = navigationService;
            this.appDataService = appDataService;
            this.localSaveService = localSaveService;
            this.SelectAnimalCommand = new Command(async a =>
            {
                if (!(a is AnimalInformationViewModel animalInfo))
                {
                    return;
                }
                if (this.appDataService.SelectedAnimal != animalInfo.AnimalInformation)
                {
                    var animal = animalInfo.AnimalInformation;

                    animal = animalInfo.AnimalInformation;

                    var trainings = await this.appDataService.Trainings;

                    foreach (var stat in animal.Statistics)
                    {
                        stat.Training = trainings.FirstOrDefault(t => t.Id == stat.TrainingId);
                    }

                    var newStatisticsTrainingIds = new List<int>();

                    foreach (var training in trainings.Where(t => animal.Statistics.All(s => s.TrainingId != t.Id)))
                    {
                        var newStat = new MriBase.Models.Models.TrainingStatistic(training);

                        newStatisticsTrainingIds.Add(training.Id);
                        animal.Statistics.Add(newStat);
                    }

                    animal.Statistics.RemoveAll(s => s.Training is null);

                    this.appDataService.SelectedAnimal = animalInfo.AnimalInformation;
                    await this.localSaveService.SaveAnimals();
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

        public Command SelectAnimalCommand { get; set; }
        public Command EditAnimalCommand { get; set; }
        public IEnumerable<AnimalInformationViewModel> Animals
        {
            get => animals;
            set
            {
                animals = value;
                OnPropertyChanged();
            }
        }
    }
}
