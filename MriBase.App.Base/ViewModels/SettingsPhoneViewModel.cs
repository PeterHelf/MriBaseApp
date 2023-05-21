using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.Views;
using MriBase.Models.Interfaces;
using MriBase.Models.Resources;
using MriBase.Models.Services.Interfaces;
using Plugin.Connectivity;
using System.IO;
using Xamarin.Forms;

namespace MriBase.App.Base.ViewModels
{
    public class SettingsPhoneViewModel : BaseViewModel
    {
        private readonly INavigationService navigationService;
        private readonly IAppDataService appDataService;
        private readonly IImageRecourceService imageRecourceService;

        public SettingsPhoneViewModel(INavigationService navigationService, IAppDataService appDataService, IImageRecourceService imageRecourceService)
        {
            this.navigationService = navigationService;
            this.appDataService = appDataService;
            this.imageRecourceService = imageRecourceService;
            this.AboutUsCommand = new Command(async () => await this.navigationService.NavigateToAsync<AboutPhonePage>());

            this.EditAnimalCommand = new Command(async () => await this.navigationService.NavigateToWithFactoryAsync<AnimalEditPageBase, IAnimalInformation>(this.appDataService.SelectedAnimal));

            this.AddAnimalCommand = new Command(async () =>
            {
                if (!this.appDataService.IsLogedInOnline)
                {
                    var returnToLogin = false;
                    await Device.InvokeOnMainThreadAsync(async () => returnToLogin = await Application.Current.MainPage.DisplayAlert(ResViewBasics.AccountNotLogedIn, ResViewAnimalRegistration.LogInToAddAnimal, ResViewBasics.Yes, ResViewBasics.No));

                    if (returnToLogin)
                    {
                        await this.navigationService.ReturnToLoginPage();
                    }
                }
                else
                {
                    await this.navigationService.NavigateToAsync<AnimalRegistrationPageBase>();
                }
            });

            this.AnimalSelectionCommand = new Command(() => this.navigationService.NavigateToAsync<AnimalSelectionPage>());

            this.LanguageCommand = new Command(() => this.navigationService.NavigateToAsync<LanguagePage>());

            this.VolumeCommand = new Command(() => this.navigationService.NavigateToAsync<VolumePage>());

            this.EditUserCommand = new Command(async () =>
            {
                if (!this.appDataService.IsLogedInOnline)
                {
                    var returnToLogin = false;
                    await Device.InvokeOnMainThreadAsync(async () => returnToLogin = await Application.Current.MainPage.DisplayAlert(ResViewBasics.AccountNotLogedIn, ResViewSettings.LoginToEditUserProfile, ResViewBasics.Yes, ResViewBasics.No));

                    if (returnToLogin)
                    {
                        await this.navigationService.ReturnToLoginPage();
                    }
                }
                else
                {
                    await this.navigationService.NavigateToAsync<UserEditPage>();
                }
            });

            this.FAQCommand = new Command(() => this.navigationService.NavigateToAsync<FAQPage>());

            this.LogOutCommand = new Command(async () =>
            {
                await this.navigationService.ReturnToLoginPage();
            });
        }

        public Command LogOutCommand { get; set; }

        public ImageSource LogOutIcon =>
            ImageSource.FromStream(() => new MemoryStream(imageRecourceService.GetImage("logout.png")));

        public Command EditUserCommand { get; set; }

        public Command FAQCommand { get; set; }

        public Command VolumeCommand { get; set; }

        public Command LanguageCommand { get; set; }

        public Command AnimalSelectionCommand { get; set; }

        public Command AddAnimalCommand { get; set; }

        public Command EditAnimalCommand { get; set; }

        public Command AboutUsCommand { get; set; }
    }
}