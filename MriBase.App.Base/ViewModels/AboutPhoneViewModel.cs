using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.Views;
using MriBase.Models.Resources;
using Xamarin.Forms;

namespace MriBase.App.Base.ViewModels
{
    public class AboutPhoneViewModel
    {
        private readonly INavigationService navigationService;

        public AboutPhoneViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;

            AppInfoCommand = new Command(() =>
            {
                this.OpenInfoPage(ResViewAbout.AppName, ResViewAbout.AppInfo);
            });

            DataProtectionCommand = new Command(() =>
            {
                this.OpenInfoPage(ResViewAbout.DataProtection, ResViewAbout.DataProtectionText);
            });

            DataTransmissionCommand = new Command(() =>
            {
                this.OpenInfoPage(ResViewAbout.DataTransmission, ResViewAbout.DataTransmissionText);
            });

            ImprintCommand = new Command(() =>
            {
                this.OpenInfoPage(ResViewAbout.Imprint, ResViewAbout.ImprintText);
            });

            TermsOfUseCommand = new Command(() =>
            {
                this.OpenInfoPage(ResViewAbout.TermsOfUse, ResViewAbout.TermsOfUseText);
            });
        }

        private void OpenInfoPage(string title, string text)
        {
            this.navigationService.NavigateToAsync(new InfoPage(title, text));
        }


        public Command AppInfoCommand { get; set; }
        public Command DataProtectionCommand { get; set; }
        public Command DataTransmissionCommand { get; set; }
        public Command ImprintCommand { get; set; }
        public Command TermsOfUseCommand { get; set; }
    }
}