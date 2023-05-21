using MriBase.App.Base.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MriBase.App.Base.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPhoneView : ContentView
    {
        public SettingsPhoneView()
        {
            InitializeComponent();

            this.BindingContext = BaseViewModel.Container.Resolve<SettingsPhoneViewModel>();
        }
    }
}