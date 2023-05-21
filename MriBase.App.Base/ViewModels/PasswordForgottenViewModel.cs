using MriBase.Models.Resources;
using MriBase.Models.Services.Interfaces;
using System;
using Xamarin.Forms;

namespace MriBase.App.Base.ViewModels
{
    public class PasswordForgottenViewModel : BaseViewModel
    {

        public Command ResetPasswordCommand { get; }

        public string Username { get; set; }

        public PasswordForgottenViewModel()
        {
            this.ResetPasswordCommand = new Command(async () =>
            {
                await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayAlert(ResViewPasswordForgotten.ServerUnavailableTitle, ResViewPasswordForgotten.ServerUnavailableText, ResViewBasics.Ok));
            });
        }
    }
}
