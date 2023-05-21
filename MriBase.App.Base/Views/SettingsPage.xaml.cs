using MriBase.App.Base.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MriBase.App.Base.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private readonly SettingsViewModelBase viewModel;

        public SettingsPage(SettingsViewModelBase viewModel)
        {
            InitializeComponent();

            this.VolumeView.IsVisible = false;
            this.LanguageView.IsVisible = true;
            BindingContext = this.viewModel = viewModel;

            viewModel.PropertyChanged += ViewModelPropertyChanged;
        }

        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(viewModel.VolumeViewIsVisible))
            {
                this.VolumeView.IsVisible = viewModel.VolumeViewIsVisible;
            }
            else if (e.PropertyName == nameof(viewModel.LanguageViewIsVisible))
            {
                this.LanguageView.IsVisible = viewModel.LanguageViewIsVisible;
            }
        }
    }
}