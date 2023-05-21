using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.Views;
using MriBase.Models.Enums;
using MriBase.Models.Resources;
using MriBase.Models.Services.Implementations;
using MriBase.Models.Services.Interfaces;
using Plugin.Connectivity;
using System;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MriBase.App.Base.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly INavigationService navigationService;
        private readonly ILoginService loginService;
        private readonly IAppDataService appDataService;
        private readonly ILocalSaveService localSaveService;
        private readonly IImageRecourceService imageRecourceService;
        private string loginErrorMessage;

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool StayLogedIn => this.appDataService.UserSettings.StayLogedIn;

        public bool ImageVisible => Device.Idiom != TargetIdiom.Phone || DeviceDisplay.MainDisplayInfo.Orientation == DisplayOrientation.Portrait;

        public ImageSource Image => ImageSource.FromStream(() => new MemoryStream(imageRecourceService.GetImage("cdlLoginScreenImage.jpeg")));

        public string LoginErrorMessage
        {
            get => loginErrorMessage;
            set
            {
                loginErrorMessage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(LoginErrorVisible));
            }
        }

        public bool LoginErrorVisible => !string.IsNullOrWhiteSpace(LoginErrorMessage);

        public LoginViewModel(INavigationService navigationService, ILoginService loginService, IContainer container, ITimedTrainingsService timedTrainingsService, IAppDataService appDataService, ILocalSaveService localSaveService, IImageRecourceService imageRecourceService)
        {
            this.navigationService = navigationService;
            this.loginService = loginService;
            this.appDataService = appDataService;
            this.localSaveService = localSaveService;
            this.imageRecourceService = imageRecourceService;
            this.Title = ResViewLogin.LoginTitle;

            DeviceDisplay.MainDisplayInfoChanged += (sender, e) => { OnPropertyChanged(nameof(this.ImageVisible)); };
#if DEBUG
            this.UserName = "TestUser";
            this.Password = "12345678";
#endif

            this.RegistrationPageCommand = new Command(async () =>
            {
                await this.navigationService.NavigateToAsync<UserRegistrationPage>();
            });

            this.PasswordForgottenCommand = new Command(async () =>
            {
                await this.navigationService.NavigateToAsync<PasswordForgottenPage>();
            });

            this.LoginCommand = new Command(async () =>
            {
                this.IsBusy = true;
                this.BusyText = ResViewLogin.LoggingIn;

                if (string.IsNullOrWhiteSpace(this.UserName) || string.IsNullOrWhiteSpace(this.Password))
                {
                    this.LoginErrorMessage = ResViewLogin.UsernameOrPasswordInvalid;
                    this.IsBusy = false;
                    return;
                }

                var loginResult = await this.loginService.LoginOffline(this.UserName, PasswordService.ComputeHash(this.Password));

                switch (loginResult)
                {
                    case LoginError.NoError:
                        this.LoginErrorMessage = string.Empty;
                        break;
                    case LoginError.FailedLogin:
                        this.LoginErrorMessage = ResViewLogin.LoginErrorText;
                        break;
                    case LoginError.ServerError:
                        this.LoginErrorMessage = ResViewLogin.ServerErrorText;
                        break;
                    case LoginError.OfflineError:
                        this.LoginErrorMessage = ResViewLogin.UserMustLoginOnlineOnce;
                        break;
                    default:
                        break;
                }

                if (loginResult != LoginError.NoError || appDataService.LogedInUser is null)
                {
                    this.IsBusy = false;
                    return;
                }

                try
                {
                    if (this.appDataService.UserSettings.StayLogedIn)
                    {
                        await SecureStorage.SetAsync("LastUserUsername", this.UserName);
                        await SecureStorage.SetAsync("LastUserPasswordHash", PasswordService.ComputeHash(this.Password));
                    }
                    else
                    {
                        SecureStorage.Remove("LastUserUsername");
                        SecureStorage.Remove("LastUserPasswordHash");
                    }
                }
                catch (Exception)
                {
                    //TODO: Errorhandling
                    // Possible that device doesn't support secure storage on device.
                }

                this.localSaveService.SaveUserSettings();
                timedTrainingsService.StartAllTimers();

                await this.navigationService.NavigateToAsync<AnimalSelectionPage>();
                this.IsBusy = false;
            });

            this.DemoLoginCommand = new Command(async () =>
            {
                this.IsBusy = true;
                this.BusyText = ResViewLogin.LoggingIn;

                var loginResult = await this.loginService.LoginOffline("TestTester", PasswordService.ComputeHash("12345678"));

                if (loginResult != LoginError.NoError || appDataService.LogedInUser is null)
                {
                    this.IsBusy = false;
                    return;
                }

                try
                {
                    if (this.appDataService.UserSettings.StayLogedIn)
                    {
                        await SecureStorage.SetAsync("LastUserUsername", this.UserName);
                        await SecureStorage.SetAsync("LastUserPasswordHash", PasswordService.ComputeHash(this.Password));
                    }
                    else
                    {
                        SecureStorage.Remove("LastUserUsername");
                        SecureStorage.Remove("LastUserPasswordHash");
                    }
                }
                catch (Exception)
                {
                    //TODO: Errorhandling
                    // Possible that device doesn't support secure storage on device.
                }

                this.localSaveService.SaveUserSettings();
                timedTrainingsService.StartAllTimers();

                await this.navigationService.NavigateToAsync<AnimalSelectionPage>();
                this.IsBusy = false;
            });
        }

        public Command PasswordForgottenCommand { get; set; }

        public Command RegistrationPageCommand { get; set; }

        public Command LoginCommand { get; set; }
        public Command DemoLoginCommand { get; set; }
    }
}