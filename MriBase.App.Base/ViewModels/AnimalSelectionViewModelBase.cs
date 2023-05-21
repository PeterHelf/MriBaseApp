using MriBase.App.Base.Bluetooth;
using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.Views;
using MriBase.Models.Interfaces;
using MriBase.Models.Models;
using MriBase.Models.Resources;
using MriBase.Models.Services.Interfaces;
using MriBase.Models.Translation;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace MriBase.App.Base.ViewModels
{
    public abstract class AnimalSelectionViewModelBase : BaseViewModel
    {
        private readonly INavigationService navigationService;
        private readonly ILocalSaveService localSaveService;
        private readonly IAppDataService appDataService;
        private readonly IImageRecourceService imageRecourceService;

        public AnimalSelectionViewModelBase(INavigationService navigationService, IBluetoothGATTServer bluetoothGATTServer, ILocalSaveService localSaveService, IAppDataService appDataService, IImageRecourceService imageRecourceService)
        {
            this.localSaveService = localSaveService;
            this.appDataService = appDataService;
            this.imageRecourceService = imageRecourceService;
            this.navigationService = navigationService;
            AddAnimalCommand = new Command(async () =>
            {
                await navigationService.NavigateToAsync<AnimalRegistrationPageBase>();
            });

            SelectAnimalCommand = new Command(async a =>
            {
                if (!(a is AnimalInformationViewModel animalInfo))
                {
                    return;
                }

                this.IsBusy = true;
                this.BusyText = ResViewBasics.Loading;

                var selectedAnimal = animalInfo.AnimalInformation;
                var trainings = await this.appDataService.Trainings;

                foreach (var stat in selectedAnimal.Statistics)
                {
                    stat.Training = trainings.FirstOrDefault(t => t.Id == stat.TrainingId);
                }

                var newStatisticsTrainingIds = new List<int>();

                foreach (var training in trainings.Where(t => selectedAnimal.Statistics.All(s => s.TrainingId != t.Id)))
                {
                    var newStat = new TrainingStatistic(training);

                    newStatisticsTrainingIds.Add(training.Id);
                    selectedAnimal.Statistics.Add(newStat);
                }

                selectedAnimal.Statistics.RemoveAll(s => s.Training is null);
                this.appDataService.SelectedAnimal = selectedAnimal;
                await this.localSaveService.SaveAnimals();

                if (Device.Idiom == TargetIdiom.Phone)
                {
                    await this.navigationService.NavigateToAsync<MainPhonePage>();
                }
                else
                {
                    await this.navigationService.NavigateToAsync<StatisticsAndTrainingPage>();

                    bluetoothGATTServer.StartServer();
                }

                this.IsBusy = false;
            });

            Animals.CollectionChanged += AnimalsCollectionChanged;

            Translator.Instance.PropertyChanged += TranslatorChanged;
        }

        private void TranslatorChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(AnimalListHeaderText));
            this.OnPropertyChanged(nameof(AnimalAddButtonText));
        }

        public abstract string AnimalListHeaderText { get; }
        public abstract string AnimalAddButtonText { get; }

        public bool UserHasAnimals => this.appDataService.Animals.Any();

        public ImageSource AddAnimalImage =>
            ImageSource.FromStream(() => new MemoryStream(imageRecourceService.GetImage("plusSymbol.png")));

        public IEnumerable<AnimalInformationViewModel> AnimalViewModels =>
           this.Animals.OrderBy(a => a.Name).Select(a => new AnimalInformationViewModel(a));

        public ObservableCollection<IAnimalInformation> Animals => this.appDataService.Animals;

        public Command AddAnimalCommand { get; set; }
        public Command SelectAnimalCommand { get; set; }

        private void AnimalsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(AnimalViewModels));
        }
    }
}