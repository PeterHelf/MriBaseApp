using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.Views;
using MriBase.Models.Interfaces;
using MriBase.Models.Resources;
using MriBase.Models.Services.Interfaces;
using MriBase.Models.Translation;
using System.IO;
using Xamarin.Forms;

namespace MriBase.App.Base.ViewModels
{
    public abstract class StatisticsAndTrainingViewModelBase : BaseViewModel
    {
        private readonly INavigationService navigationService;
        private readonly IFeederService feederService;
        private readonly IAppDataService appDataService;
        private readonly IImageRecourceService imageRecourceService;

        public StatisticsAndTrainingViewModelBase(INavigationService navigationService, IFeederService feederService, IAppDataService appDataService, IImageRecourceService imageRecourceService)
        {
            this.navigationService = navigationService;
            this.feederService = feederService;
            this.appDataService = appDataService;
            this.imageRecourceService = imageRecourceService;
            this.AnimalInfo = new AnimalInformationViewModel(this.appDataService.SelectedAnimal);
            this.AnimalSelectionCommand = new Command(() => this.navigationService.NavigateToAsync<AnimalSelectionPage>());
            this.SettingsClickedCommand = new Command(() => this.navigationService.NavigateToAsync<SettingsPage>());
            this.AnimalButtonCommand = new Command(() => this.navigationService.NavigateToWithFactoryAsync<AnimalEditPageBase, IAnimalInformation>(appDataService.SelectedAnimal));
            this.FeederScanStartCommand = new Command(async () =>
            {
                if (!this.feederService.FeederDeviceConnected)
                {
                    this.feederService.ScanForDevices();
                }
                else
                {
                    var disconnect = false;
                    await Device.InvokeOnMainThreadAsync(async () => disconnect = await Application.Current.MainPage.DisplayAlert(ResViewStatisticsAndTraining.Disconnect, ResViewStatisticsAndTraining.DoYouWantToDisconnect, ResViewBasics.Yes, ResViewBasics.No));

                    if (disconnect)
                    {
                        await this.feederService.DisconnectFeeder();
                    }
                }
            });

            this.feederService.FeederConnected += FeederConnectionChanged;
            this.feederService.FeederDisconnected += FeederConnectionChanged;
            this.feederService.FeederScanStarted += FeederScanStatusChanged;
            this.feederService.FeederScanStopped += FeederScanStatusChanged;
            Translator.Instance.PropertyChanged += TranslatorChanged;

            this.feederService.ScanForDevices();
        }

        private void TranslatorChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(AnimalSelectionText));
        }

        public abstract string AnimalSelectionText { get; }

        public bool FeederScanning => this.feederService.IsScanning;

        public Command AnimalButtonCommand { get; set; }
        public Command FeederScanStartCommand { get; set; }
        public Command SettingsClickedCommand { get; set; }

        public Command AnimalSelectionCommand { get; set; }

        public Color FeederConnectedColor
        {
            get
            {
                if (this.feederService.FeederDeviceConnected)
                {
                    return Color.Green;
                }
                else
                {
                    return Color.Red;
                }
            }
        }

        public AnimalInformationViewModel AnimalInfo { get; set; }

        public ImageSource CogIcon => ImageSource.FromStream(() => new MemoryStream(imageRecourceService.GetImage("cog.png")));

        public void Refresh()
        {
            this.OnPropertyChanged(nameof(this.AnimalInfo));
        }

        private void FeederConnectionChanged(object sender, System.EventArgs e)
        {
            this.OnPropertyChanged(nameof(this.FeederConnectedColor));
        }

        private void FeederScanStatusChanged(object sender, System.EventArgs e)
        {
            this.OnPropertyChanged(nameof(this.FeederScanning));
        }
    }
}