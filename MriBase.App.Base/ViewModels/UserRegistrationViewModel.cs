using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.Views;
using MriBase.Models.Enums;
using MriBase.Models.Resources;
using MriBase.Models.Services.Implementations;
using MriBase.Models.Services.Interfaces;
using System;
using Xamarin.Forms;

namespace MriBase.App.Base.ViewModels
{
    public class UserRegistrationViewModel : BaseViewModel
    {
        private readonly INavigationService navigationService;
        private readonly IAppDataService appDataService;

        public UserRegistrationViewModel(INavigationService navigationService, IAppDataService appDataService)
        {
            this.navigationService = navigationService;
            this.appDataService = appDataService;
            this.RegisterCommand = new Command(async () =>
            {
                if (!this.TermsAndConditionsAccepted)
                {
                    Device.BeginInvokeOnMainThread(async () => await Application.Current.MainPage.DisplayAlert(ResViewRegistration.TermsAndConditionsTitle, ResViewRegistration.TermsAndConditionsText, ResViewBasics.Ok));
                    return;
                }

                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(PasswordRepeated) || string.IsNullOrWhiteSpace(Email))
                {
                    Device.BeginInvokeOnMainThread(async () => await Application.Current.MainPage.DisplayAlert(ResViewRegistration.DataIncompleteTitle, ResViewRegistration.DataIncompleteText, ResViewBasics.Ok));
                    return;
                }

                if (string.IsNullOrWhiteSpace(this.Password) || this.Password.Length < 8)
                {
                    Device.BeginInvokeOnMainThread(async () => await Application.Current.MainPage.DisplayAlert(ResViewRegistration.PasswordTooShortTitle, ResViewRegistration.PasswordTooShortText, ResViewBasics.Ok));
                    return;
                }

                if (Password != PasswordRepeated)
                {
                    Device.BeginInvokeOnMainThread(async () => await Application.Current.MainPage.DisplayAlert(ResViewRegistration.PasswordErrorTitle, ResViewRegistration.PasswordErrorText, ResViewBasics.Ok));
                    return;
                }

                if (!IsValidEmail(Email))
                {
                    Device.BeginInvokeOnMainThread(async () => await Application.Current.MainPage.DisplayAlert(ResViewRegistration.EmailInvalidTitle, ResViewRegistration.EmailInvalidText, ResViewBasics.Ok));
                    return;
                }

                this.IsBusy = true;
                this.BusyText = ResViewRegistration.Registering;

                Device.BeginInvokeOnMainThread(async () => await Application.Current.MainPage.DisplayAlert(ResViewRegistration.ServerUnavailableTitle, ResViewRegistration.ServerUnavailableText, ResViewBasics.Ok));

                this.IsBusy = false;
            });

            this.DataProtectionCommand = new Command(() =>
            {
                this.navigationService.NavigateToAsync(new InfoPage(ResViewAbout.DataProtection, ResViewAbout.DataProtectionText));
            });

            this.TermsOfServiceCommand = new Command(() =>
            {
                this.navigationService.NavigateToAsync(new InfoPage(ResViewAbout.TermsOfUse, ResViewAbout.TermsOfUseText));
            });
        }

        public Command TermsOfServiceCommand { get; set; }

        public Command DataProtectionCommand { get; set; }

        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public bool TermsAndConditionsAccepted { get; set; }

        public Command RegisterCommand { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string PasswordRepeated { get; set; }

        public string Email { get; set; }
    }
}