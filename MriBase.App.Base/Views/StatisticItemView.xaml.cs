using MriBase.App.Base.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MriBase.App.Base.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatisticItemView : ContentView
    {
        public static readonly BindableProperty StatisticProperty =
            BindableProperty.Create(nameof(Statistic), typeof(TrainingsStatisticViewModel), typeof(StatisticItemView), propertyChanged: OnStatisticPropertyChanged);

        public TrainingsStatisticViewModel Statistic
        {
            get { return base.GetValue(StatisticProperty) as TrainingsStatisticViewModel; }
            set { base.SetValue(StatisticProperty, value); }
        }

        public StatisticItemView()
        {
            InitializeComponent();
        }

        static void OnStatisticPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((StatisticItemView)bindable).OnStatisticPropertyChanged(newValue as TrainingsStatisticViewModel);
        }

        void OnStatisticPropertyChanged(TrainingsStatisticViewModel newValue)
        {
            this.BindingContext = newValue;
        }
    }
}