using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MriBase.App.Base.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VolumeView : ContentView
    {
        private readonly ILocalSaveService localSaveService;
        private readonly IAppDataService appDataService;

        public VolumeView()
        {
            InitializeComponent();

            this.localSaveService = BaseViewModel.Container.Resolve<ILocalSaveService>();
            this.appDataService = BaseViewModel.Container.Resolve<IAppDataService>();
            BindingContext = this;
        }

        private void SaveUserSettings()
        {
            this.localSaveService.SaveUserSettings();
        }

        public float Volume
        {
            get => this.appDataService.UserSettings.Volume;
            set
            {
                this.appDataService.UserSettings.Volume = value;
                this.SaveUserSettings();
            }
        }
    }
}