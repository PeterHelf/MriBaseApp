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

        public SettingsPhoneViewModel(INavigationService navigationService, IAppDataService appDataService)
        {
            this.navigationService = navigationService;
            this.appDataService = appDataService;

            this.EditAnimalCommand = new Command(async () => await this.navigationService.NavigateToWithFactoryAsync<AnimalEditPageBase, IAnimalInformation>(this.appDataService.SelectedAnimal));

            this.AddAnimalCommand = new Command(async () =>
            {
                await this.navigationService.NavigateToAsync<AnimalRegistrationPageBase>();
            });

            this.AnimalSelectionCommand = new Command(() => this.navigationService.NavigateToAsync<AnimalSelectionPage>());

            this.LanguageCommand = new Command(() => this.navigationService.NavigateToAsync<LanguagePage>());

            this.VolumeCommand = new Command(() => this.navigationService.NavigateToAsync<VolumePage>());
        }


        public Command VolumeCommand { get; set; }

        public Command LanguageCommand { get; set; }

        public Command AnimalSelectionCommand { get; set; }

        public Command AddAnimalCommand { get; set; }

        public Command EditAnimalCommand { get; set; }
    }
}