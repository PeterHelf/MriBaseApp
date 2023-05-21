using MriBase.App.Base.Services.Interfaces;
using MriBase.Models.Resources;
using Xamarin.Forms;

namespace MriBase.App.Base.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private string text;

        public AboutViewModel(INavigationService navigationService)
        {
            AppInfoCommand = new Command(() =>
            {
                Title = ResViewAbout.AppName;
                Text = ResViewAbout.AppInfo;
            });

            DataProtectionCommand = new Command(() =>
            {
                Title = ResViewAbout.DataProtection;
                Text = ResViewAbout.DataProtectionText;
            });

            DataTransmissionCommand = new Command(() =>
            {
                Title = ResViewAbout.DataTransmission;
                Text = ResViewAbout.DataTransmissionText;
            });

            ImprintCommand = new Command(() =>
            {
                Title = ResViewAbout.Imprint;
                Text = ResViewAbout.ImprintText;
            });

            TermsOfUseCommand = new Command(() =>
            {
                Title = ResViewAbout.TermsOfUse;
                Text = ResViewAbout.TermsOfUseText;
            });

            BackCommand = new Command(() => { navigationService.ReturnToLastPage(); });

            AppInfoCommand.Execute(null);
        }

        public string Text
        {
            get => this.text;
            set
            {
                this.text = value;
                OnPropertyChanged();
            }
        }

        public Command AppInfoCommand { get; set; }
        public Command DataProtectionCommand { get; set; }
        public Command DataTransmissionCommand { get; set; }
        public Command ImprintCommand { get; set; }
        public Command TermsOfUseCommand { get; set; }
        public Command BackCommand { get; set; }
    }
}