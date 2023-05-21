
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MriBase.App.Base.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPage : ContentPage
    {
        public string Text { get; }

        public InfoPage(string title, string text)
        {
            Title = title;
            Text = text;
            InitializeComponent();

            this.BindingContext = this;
        }
    }
}