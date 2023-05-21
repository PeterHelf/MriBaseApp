
using MriBase.App.Base.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MriBase.App.Base.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PasswordForgottenPage : ContentPage
    {
        public PasswordForgottenPage(PasswordForgottenViewModel viewModel)
        {
            InitializeComponent();

            this.BindingContext = viewModel;
        }
    }
}