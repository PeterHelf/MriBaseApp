using Microcharts;
using MriBase.App.Base.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MriBase.App.Base.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrainingStatisticDetailsPage : ContentPage
    {
        private readonly TrainingStatisticDetailsViewModel viewModel;

        public TrainingStatisticDetailsPage(TrainingStatisticDetailsViewModel viewModel)
        {
            InitializeComponent();
            this.BindingContext = this.viewModel = viewModel;

            groupingPicker.SelectedIndex = 1;

            this.viewModel.PropertyChanged += this.ChartChanged;
        }


        private void ChartChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() => this.UpdateCharts());
        }

        private void UpdateCharts()
        {
            chart1.Chart = new BarChart() { Entries = viewModel.FailureChartEntries, LabelTextSize = 30, LabelOrientation = Orientation.Vertical };
            chart2.Chart = new BarChart() { Entries = viewModel.TimeChartEntries, LabelTextSize = 30, LabelOrientation = Orientation.Vertical };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.UpdateCharts();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            var chartWidth = chart1.Width;
            chart1.HeightRequest = chartWidth / 2.6 + 80;
            chartWidth = chart2.Width;
            chart2.HeightRequest = chartWidth / 2.6 + 80;
        }
    }
}