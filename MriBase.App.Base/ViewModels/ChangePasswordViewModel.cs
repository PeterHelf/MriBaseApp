using MriBase.App.Base.Services.Interfaces;
using MriBase.Models.Enums;
using MriBase.Models.Resources;
using MriBase.Models.Services.Implementations;
using MriBase.Models.Services.Interfaces;
using System;
using Xamarin.Forms;

namespace MriBase.App.Base.ViewModels
{
    public class ChangePasswordViewModel : BaseViewModel
    {
        private readonly INavigationService navigationService;
        private readonly IAppDataService appDataService;
        private readonly ILocalSaveService localSaveService;

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        public string NewPasswordRepeated { get; set; }

        public Command SavePasswordCommand { get; set; }

        public ChangePasswordViewModel(INavigationService navigationService,IAppDataService appDataService, ILocalSaveService localSaveService)
        {
            this.navigationService = navigationService;
            this.appDataService = appDataService;
            this.localSaveService = localSaveService;
            this.SavePasswordCommand = new Command(async () =>
            {
                if (string.IsNullOrWhiteSpace(this.NewPassword) || this.NewPassword.Length < 8)
                {
                    Device.BeginInvokeOnMainThread(async () => await Application.Current.MainPage.DisplayAlert(ResViewChangePassword.PasswordTooShortTitle, ResViewChangePassword.PasswordTooShortText, ResViewBasics.Ok));
                    return;
                }

                if (string.IsNullOrWhiteSpace(this.CurrentPassword))
                {
                    Device.BeginInvokeOnMainThread(async () => await Application.Current.MainPage.DisplayAlert(ResViewChangePassword.WrongPasswordTitle, ResViewChangePassword.WrongPasswordText, ResViewBasics.Ok));
                    return;
                }

                if (this.NewPassword != this.NewPasswordRepeated)
                {
                    Device.BeginInvokeOnMainThread(async () => await Application.Current.MainPage.DisplayAlert(ResViewChangePassword.PasswordMatchingErrorTitle, ResViewChangePassword.PasswordMatchingErrorText, ResViewBasics.Ok));
                    return;
                }

                this.IsBusy = true;
                this.BusyText = ResViewChangePassword.SavingChanges;

                Device.BeginInvokeOnMainThread(async () => await Application.Current.MainPage.DisplayAlert(ResViewChangePassword.ServerUnavailableTitle, ResViewChangePassword.ServerUnavailableText, ResViewBasics.Ok));

                this.IsBusy = false;
            });
        }
    }
}
