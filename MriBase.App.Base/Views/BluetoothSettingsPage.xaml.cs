using MriBase.App.Base.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MriBase.App.Base.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BluetoothSettingsPage : ContentPage
    {
        public BluetoothSettingsPage(BluetoothSettingsViewModel viewModel)
        {
            InitializeComponent();

            this.BindingContext = viewModel;
        }
    }
}