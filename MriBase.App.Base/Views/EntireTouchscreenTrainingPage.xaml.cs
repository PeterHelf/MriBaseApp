using MriBase.App.Base.ViewModels;
using Xamarin.Forms.Xaml;

namespace MriBase.App.Base.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntireTouchscreenTrainingPage : BaseTrainingPage
    {
        public EntireTouchscreenTrainingPage(EntireTouchscreenTrainingViewModel viewModel)
          : base(viewModel)
        {
            InitializeComponent();
        }
    }
}