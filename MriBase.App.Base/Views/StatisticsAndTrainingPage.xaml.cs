using MriBase.App.Base.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MriBase.App.Base.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatisticsAndTrainingPage : ContentPage
    {
        private readonly StatisticsAndTrainingViewModelBase viewModel;

        public StatisticsAndTrainingPage(StatisticsAndTrainingViewModelBase viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        //HACK
        protected override void OnAppearing()
        {
            this.viewModel.Refresh();
        }
    }
}