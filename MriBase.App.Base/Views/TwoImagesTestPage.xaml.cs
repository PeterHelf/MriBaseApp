using MriBase.App.Base.ViewModels;
using Xamarin.Forms.Xaml;

namespace MriBase.App.Base.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TwoImagesTestPage : BaseTrainingPage
    {
        public TwoImagesTestPage(TwoImagesTestViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}