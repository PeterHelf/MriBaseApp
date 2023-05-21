using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.Views;
using MriBase.Models.Enums;
using MriBase.Models.Resources;
using MriBase.Models.Services.Interfaces;
using System;
using Xamarin.Forms;

namespace MriBase.App.Base.ViewModels
{
    public class UserEditViewModel : BaseViewModel
    {
        private readonly INavigationService navigationService;
        private readonly IAppDataService appDataService;
        private readonly ILocalSaveService localSaveService;

        public UserEditViewModel(INavigationService navigationService, IAppDataService appDataService, ILocalSaveService localSaveService)
        {
            this.navigationService = navigationService;
            this.appDataService = appDataService;
            this.localSaveService = localSaveService;
            this.Email = this.appDataService.LogedInUser.Email;
            this.Username = this.appDataService.LogedInUser.UserName;

            this.ChangePasswordCommand = new Command(() => this.navigationService.NavigateToAsync<ChangePasswordPage>());

            this.SaveChangesCommand = new Command(async () =>
            {
                if (!IsValidEmail(Email))
                {
                    Device.BeginInvokeOnMainThread(async () => await Application.Current.MainPage.DisplayAlert(ResViewRegistration.EmailInvalidTitle, ResViewRegistration.EmailInvalidText, ResViewBasics.Ok));
                    return;
                }

                if (string.IsNullOrWhiteSpace(Username))
                {
                    Device.BeginInvokeOnMainThread(async () => await Application.Current.MainPage.DisplayAlert(ResViewUserEdit.UsernameEmptyTitle, ResViewUserEdit.UsernameEmptyText, ResViewBasics.Ok));
                    return;
                }

                this.IsBusy = true;
                this.BusyText = ResViewUserEdit.SavingChanges;

                Device.BeginInvokeOnMainThread(async () => await Application.Current.MainPage.DisplayAlert(ResViewUserEdit.ServerUnavailableTitle, ResViewUserEdit.ServerUnavailableText, ResViewBasics.Ok));

                this.IsBusy = false;
            });
        }

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

        public Command ChangePasswordCommand { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public Command SaveChangesCommand { get; set; }
    }
}