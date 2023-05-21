using MriBase.App.Base.ExtensionMethods;
using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.ViewModels;
using MriBase.Models.Enums;
using MriBase.Models.Models;
using MriBase.Models.Resources;
using MriBase.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace MriBase.App.Dog.ViewModels
{
    public class AnimalRegistrationViewModel : AnimalRegistrationViewModelBase
    {
        public Breed? SelectedBreed { get; set; }
        public IEnumerable<AnimalBreedViewModel> Breeds { get; }
        public IEnumerable<string> BreedStrings { get; }
        public override Command SaveCommand { get; set; }
        public Func<string, ICollection<string>, ICollection<string>> SortingAlgorithm { get; }

        public AnimalRegistrationViewModel(INavigationService navigationService,IAppDataService appDataService, ILocalSaveService localSaveService, IImageRecourceService imageRecourceService) : base(navigationService, appDataService, localSaveService, imageRecourceService)
        {
            this.Breeds = Enum.GetValues(typeof(Breed)).Cast<Breed>().Select(b => new AnimalBreedViewModel(b));
            this.BreedStrings = this.Breeds.Select(b => b.BreedName);
            this.SortingAlgorithm = (text, values) =>
                values.Where(x => x.Contains(text, StringComparison.OrdinalIgnoreCase)).OrderBy(x => x)
                    .ToList();

            this.SaveCommand = new Command(async () =>
            {
                if (Name != string.Empty && this.SelectedGender.HasValue && SelectedBreed.HasValue)
                {
                    this.IsBusy = true;
                    this.BusyText = ResViewAnimalRegistration.Saving;

                    DateTime dateOfBirth = this.SelectedDate;

                    var newAnimal = new DogInformation(this.Name, this.appDataService.LogedInUser.UserId, this.ImageBytes, dateOfBirth, this.SelectedGender.Value,
                        SelectedBreed.Value);

                    var rnd = new Random();
                    newAnimal.Id = rnd.Next(int.MaxValue);
                    this.appDataService.Animals.Add(newAnimal);
                    await this.localSaveService.SaveAnimals();
                    await this.navigationService.ReturnToLastPage(true);

                    this.IsBusy = false;
                }
                else
                {
                    await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayAlert(ResViewAnimalRegistration.DataIncompleteTitle, ResViewAnimalRegistration.DataIncompleteText, ResViewBasics.Ok));
                }
            });
        }
    }
}