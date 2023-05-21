using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.Views;
using MriBase.Models.Services.Interfaces;
using System.IO;
using Xamarin.Forms;

namespace MriBase.App.Base.ViewModels
{
    public abstract class SettingsViewModelBase : BaseViewModel
    {
        private readonly INavigationService navigationService;
        private readonly IImageRecourceService imageRecourceService;
        private bool languageViewIsVisible;
        private bool volumeViewIsVisible;

        public SettingsViewModelBase(INavigationService navigationService, IImageRecourceService imageRecourceService)
        {
            this.LanguageViewIsVisible = true;
            this.VolumeViewIsVisible = false;
            this.navigationService = navigationService;
            this.imageRecourceService = imageRecourceService;
            BackCommand = new Command(() => { this.navigationService.ReturnToLastPage(true); });

            LogOutCommand = new Command(async () =>
            {
                await this.navigationService.ReturnToLoginPage();
            });

            LanguageButtonCommand = new Command(() =>
            {
                LanguageViewIsVisible = true;
                VolumeViewIsVisible = false;
            });

            VolumeButtonCommand = new Command(() =>
            {
                VolumeViewIsVisible = true;
                LanguageViewIsVisible = false;
            });

            DailyTrainingsButtonCommand = new Command(() => this.navigationService.NavigateToAsync<DailyTrainingsPage>());

            TouchscreenCalibrationButtonCommand = new Command(() => this.navigationService.NavigateToAsync<TouchscreenCalibrationPage>());
        }

        public abstract bool LogoutPossible { get; }

        public Command LogOutCommand { get; set; }
        public Command LanguageButtonCommand { get; }
        public Command VolumeButtonCommand { get; }
        public Command DailyTrainingsButtonCommand { get; }
        public Command TouchscreenCalibrationButtonCommand { get; }        
        public Command BackCommand { get; set; }

        public ImageSource LogOutIcon =>
            ImageSource.FromStream(() => new MemoryStream(imageRecourceService.GetImage("logout.png")));

        public bool LanguageViewIsVisible
        {
            get => languageViewIsVisible;

            private set
            {
                languageViewIsVisible = value;
                this.OnPropertyChanged();
            }
        }

        public bool VolumeViewIsVisible
        {
            get => volumeViewIsVisible;

            private set
            {
                volumeViewIsVisible = value;
                this.OnPropertyChanged();
            }
        }
    }
}