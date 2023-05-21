using MriBase.App.Base.Converter;
using MriBase.App.Base.ExtensionMethods;
using MriBase.App.Base.Services.Interfaces;
using MriBase.Models.Enums;
using MriBase.Models.Interfaces;
using MriBase.Models.Models;
using MriBase.Models.Resources;
using MriBase.Models.Services.Interfaces;
using Plugin.Connectivity;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace MriBase.App.Base.ViewModels
{
    public class AnimalEditViewModel : BaseViewModel
    {
        private readonly IAppDataService appDataService;
        private readonly INavigationService navigationService;
        private readonly IOfflineChangesManager offlineChangesManager;
        private readonly ILocalSaveService localSaveService;
        private byte[] imageBytes;
        private bool imageChanged;
        public IEnumerable<AnimalBreedViewModel> Breeds { get; }
        public IEnumerable<string> BreedStrings { get; }
        public Func<string, ICollection<string>, ICollection<string>> SortingAlgorithm { get; }
        public string Name { get; set; }
        public Command SaveCommand { get; set; }
        public Command SetFemaleCommand { get; }
        public Command SetMaleCommand { get; }
        public Breed? SelectedBreed { get; set; }
        public Gender? SelectedSex { get; set; }

        public DateTime MaxDate => DateTime.Today;
        public DateTime MinDate => DateTime.Today.AddYears(-30);
        public DateTime SelectedDate { get; set; }

        public ImageSource AnimalImage => ImageSource.FromStream(() => new MemoryStream(ImageBytes));

        public byte[] ImageBytes
        {
            get => imageBytes;
            set
            {
                imageBytes = value;
                this.OnPropertyChanged(nameof(AnimalImage));
            }
        }

        public AnimalEditViewModel(IAppDataService appDataService, INavigationService navigationService, IOfflineChangesManager offlineChangesManager, IAnimalInformation animalInfo, ILocalSaveService localSaveService)
        {
            this.appDataService = appDataService;
            this.navigationService = navigationService;
            this.offlineChangesManager = offlineChangesManager;
            this.localSaveService = localSaveService;
            this.Name = animalInfo.Name;
            this.SelectedDate = animalInfo.DateOfBirth;
            this.SelectedSex = animalInfo.Sex;

            this.Breeds = Enum.GetValues(typeof(Breed)).Cast<Breed>().Select(b => new AnimalBreedViewModel(b));
            this.BreedStrings = this.Breeds.Select(b => b.BreedName);
            this.SortingAlgorithm = (text, values) =>
                values.Where(x => x.Contains(text, StringComparison.OrdinalIgnoreCase)).OrderBy(x => x)
                    .ToList();

            if (animalInfo is DogInformation dogInformation)
            {
                this.SelectedBreed = dogInformation.Breed;
            }

            this.ImageBytes = animalInfo.Image;

            this.ChangeAnimalImageCommand = new Command(async () =>
            {
                var photo = await CrossMedia.Current.PickPhotoAsync();
                if (!(photo is null))
                {
                    using (var stream = photo.GetStream())
                    {
                        var memoryStream = new MemoryStream();
                        stream.CopyTo(memoryStream);
                        this.ImageBytes = memoryStream.ToArray();
                        this.imageChanged = true;
                    }
                }
            });

            this.SaveCommand = new Command(async () =>
            {
                if (Name != string.Empty && this.SelectedSex.HasValue && (!(animalInfo is DogInformation) || this.SelectedBreed.HasValue))
                {
                    this.IsBusy = true;
                    this.BusyText = ResViewAnimalEdit.SavingChanges;

                    animalInfo.Name = this.Name;
                    animalInfo.Image = this.ImageBytes;
                    animalInfo.DateOfBirth = this.SelectedDate;
                    animalInfo.Sex = this.SelectedSex.Value;

                    if (animalInfo is DogInformation dogInfo)
                    {
                        dogInfo.Breed = this.SelectedBreed.Value;
                    }

                    await this.localSaveService.SaveAnimals();

                    this.offlineChangesManager.AddChangedAnimal(animalInfo.Id);
                    await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayAlert(ResViewAnimalEdit.ChangesSaved, ResViewAnimalEdit.ChangesSavedText, ResViewBasics.Ok));
                    await this.navigationService.ReturnToLastPage(true);
                }
                else
                {
                    await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayAlert(ResViewAnimalEdit.NameEmptyErrorTitle, ResViewAnimalEdit.NameEmptyErrorText, ResViewBasics.Ok));
                }

                this.IsBusy = false;
            });

            this.SetFemaleCommand = new Command(() => { this.SelectedSex = Gender.Female; });
            this.SetMaleCommand = new Command(() => { this.SelectedSex = Gender.Male; });
        }

        public Command ChangeAnimalImageCommand { get; set; }
    }
}