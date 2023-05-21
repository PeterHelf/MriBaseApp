using MriBase.App.Base.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MriBase.App.Base.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatisticsView : ContentView
    {
        public StatisticsView()
        {
            InitializeComponent();

            this.BindingContext = BaseViewModel.Container.Resolve<StatisticsViewModel>();
        }
    }
}