
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MriBase.App.Base.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BusyIndicatorView : ContentView
    {
        public static readonly BindableProperty IsBusyProperty =
            BindableProperty.Create("IsBusy", typeof(bool), typeof(BusyIndicatorView));

        public static readonly BindableProperty BusyTextProperty =
            BindableProperty.Create("BusyText", typeof(string), typeof(BusyIndicatorView));

        public bool IsBusy
        {
            get { return (bool)base.GetValue(IsBusyProperty); }
            set { base.SetValue(IsBusyProperty, value); }
        }

        public string BusyText
        {
            get { return base.GetValue(BusyTextProperty).ToString(); }
            set { base.SetValue(BusyTextProperty, value); }
        }

        public BusyIndicatorView()
        {
            InitializeComponent();
        }
    }
}