using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.ViewModels;
using MriBase.App.Base.Views;
using MriBase.App.Dog.Services.Implementations;
using MriBase.Models.Models;
using MriBase.Models.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MriBase.App.Dog
{
    public partial class App : Application
    {
        private readonly IContainer container;
        private readonly IAppDataService appDataService;
        private readonly ILocalSaveService localSaveService;

        public App()
        {
            InitializeComponent();

            //TODO: Experimental feature
            Device.SetFlags(new string[] { "MediaElement_Experimental", "SwipeView_Experimental" });

            this.container = new Container();
            BaseViewModel.Container = container.Resolve<IContainer>();

            this.localSaveService = container.Resolve<ILocalSaveService>();
            this.appDataService = container.Resolve<IAppDataService>();

            this.appDataService.UserSettings = localSaveService.LoadUserSettings();
            this.appDataService.UserSettings.UpdateAppLanguage();

            this.Initialize();

            if (Device.RuntimePlatform == Device.Android)
            {
                this.MainPage = new NavigationPage();
            }

            this.AutoLogin();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private async void AutoLogin()
        {
            this.MainPage = new NavigationPage();

            var loginManager = this.container.Resolve<ILoginService>();
            await loginManager.LoginOffline("DemoUser", "Jjdv@SM$gwV5snWk");

            var timedTraingsService = this.container.Resolve<ITimedTrainingsService>();
            timedTraingsService.StartAllTimers();

            var navigationService = this.container.Resolve<INavigationService>();
            await navigationService.NavigateToAsync<AnimalSelectionPage>();
        }

        private void Initialize()
        {
            this.appDataService.Trainings = GetTrainingsSets();
        }

        private async Task<Training[]> GetTrainingsSets()
        {
            var sets = this.localSaveService.LoadTrainings();

            return sets.ToArray();
        }
    }
}
