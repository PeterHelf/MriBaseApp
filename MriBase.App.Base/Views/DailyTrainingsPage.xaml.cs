using MriBase.App.Base.ViewModels;
using MriBase.Models.Resources;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MriBase.App.Base.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DailyTrainingsPage : ContentPage
    {
        private readonly DailyTrainingsViewModel viewModel;

        public DailyTrainingsPage(DailyTrainingsViewModel viewModel)
        {
            InitializeComponent();

            this.BindingContext = this.viewModel = viewModel;
        }

        protected override void OnAppearing()
        {
            this.viewModel.Refresh();
        }

        private async void InfoButtonClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Info", ResViewDailyTrainings.TrainingInfo, ResViewBasics.Ok);
        }
    }
}