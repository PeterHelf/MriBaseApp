using MriBase.App.Base.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MriBase.App.Base.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LanguageView : ContentView
    {
        public LanguageView()
        {
            InitializeComponent();

            BindingContext = BaseViewModel.Container.Resolve<LanguageSelectionViewModel>();
        }
    }
}