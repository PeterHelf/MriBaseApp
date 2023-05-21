using MriBase.App.Base.Services.Interfaces;
using MriBase.Models.Enums;
using MriBase.Models.Services.Interfaces;
using Plugin.Media;
using System;
using System.IO;
using Xamarin.Forms;

namespace MriBase.App.Base.ViewModels
{
    public abstract class AnimalRegistrationViewModelBase : BaseViewModel
    {
        protected readonly INavigationService navigationService;
        protected readonly IAppDataService appDataService;
        protected readonly ILocalSaveService localSaveService;

        private byte[] _imageBytes;

        public AnimalRegistrationViewModelBase(INavigationService navigationService, IAppDataService appDataService, ILocalSaveService localSaveService, IImageRecourceService imageRecourceService)
        {
            this.navigationService = navigationService;
            this.appDataService = appDataService;
            this.localSaveService = localSaveService;
            this.ImageBytes = imageRecourceService.GetDefaultAnimalImage();
            this.SelectedDate = DateTime.Today;

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
                    }
                }
            });

            this.SetFemaleCommand = new Command(() => { this.SelectedGender = Gender.Female; });
            this.SetMaleCommand = new Command(() => { this.SelectedGender = Gender.Male; });
        }

        public string Name { get; set; }
        public Gender? SelectedGender { get; set; }
        public abstract Command SaveCommand { get; set; }
        public Command ChangeAnimalImageCommand { get; set; }

        public ImageSource AnimalImage => ImageSource.FromStream(() => new MemoryStream(ImageBytes));

        public DateTime MaxDate => DateTime.Today;

        public DateTime MinDate => DateTime.Today.AddYears(-30);

        public DateTime SelectedDate { get; set; }

        public Command SetMaleCommand { get; set; }

        public Command SetFemaleCommand { get; set; }

        public byte[] ImageBytes
        {
            get => _imageBytes;
            set
            {
                _imageBytes = value;
                this.OnPropertyChanged(nameof(AnimalImage));
            }
        }
    }
}