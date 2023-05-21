using MriBase.App.Base.Bluetooth;
using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.ViewModels;
using MriBase.Models.Models;
using MriBase.Models.Services.Interfaces;
using Xamarin.Forms.Xaml;

namespace MriBase.App.Base.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeathRecognitionTraining1Page : BaseTrainingPage
    {
        public DeathRecognitionTraining1Page(DeathRecognitionTraining1ViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}