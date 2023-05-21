using MriBase.App.Base.Bluetooth;
using MriBase.App.Base.Custom;
using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.ViewModels;
using MriBase.Models.Models;
using MriBase.Models.Services.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MriBase.App.Base.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RandomPositionTestPage : BaseTrainingPage
    {
        private readonly RandomPositionTestViewModel viewModel;

        public RandomPositionTestPage(RandomPositionTestViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();

            this.viewModel = viewModel;

            this.FillGrid();
            this.viewModel.PropertyChanged += CurrentImagesChanged;
        }

        private void CurrentImagesChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.viewModel.ImagesVisible) && this.viewModel.ImagesVisible)
            {
                this.FillGrid();
            }
        }

        private void FillGrid()
        {
            this.MainGrid.Children.Clear();

            foreach (var image in this.viewModel.CurrentImages)
            {
                var img = new Image
                {
                    Source = image.Image
                };

                SimplePressedEffect.SetPressedCommand(img, this.viewModel.ImageClickCommand);
                SimplePressedEffect.SetParameter(img, image);

                img.Effects.Add(new SimplePressedEffect());

                this.MainGrid.Children.Add(img, image.Position.X, image.Position.Y);
            }
        }
    }
}