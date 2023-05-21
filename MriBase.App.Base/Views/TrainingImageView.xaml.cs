using MriBase.App.Base.ViewModels;
using System.Runtime.CompilerServices;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MriBase.App.Base.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrainingImageView : ContentView
    {
        public static readonly BindableProperty TrainingImageProperty =
            BindableProperty.Create("TrainingImage", typeof(TrainingImageViewModel), typeof(TrainingImageView));

        public static readonly BindableProperty ImageClickCommandProperty =
            BindableProperty.Create("ImageClickCommand", typeof(Command), typeof(TrainingImageView));

        public TrainingImageViewModel TrainingImage
        {
            get { return (TrainingImageViewModel)base.GetValue(TrainingImageProperty); }
            set { base.SetValue(TrainingImageProperty, value); }
        }

        public Command ImageClickCommand
        {
            get { return (Command)base.GetValue(ImageClickCommandProperty); }
            set { base.SetValue(ImageClickCommandProperty, value); }
        }

        public TrainingImageView()
        {
            InitializeComponent();

            //this.BindingContext = this;
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(TrainingImage))
            {
                this.ChangeGridSizes();
            }
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            this.ChangeGridSizes();
        }

        private void ChangeGridSizes()
        {
            if (this.TrainingImage != null)
            {
                var total = 150;

                var imageSize = TrainingImage.ImageSize * 100;
                var imageTouchableBorderSize = TrainingImage.ImageTouchableBorderSize * 100;

                var remaining = total - imageSize - (2 * imageTouchableBorderSize);

                this.OuterGrid.RowDefinitions[0].Height = new GridLength(remaining / 2, GridUnitType.Star);
                this.OuterGrid.RowDefinitions[1].Height = new GridLength(imageSize + (2 * imageTouchableBorderSize), GridUnitType.Star);
                this.OuterGrid.RowDefinitions[2].Height = new GridLength(remaining / 2, GridUnitType.Star);

                this.OuterGrid.ColumnDefinitions[0].Width = new GridLength(remaining / 2, GridUnitType.Star);
                this.OuterGrid.ColumnDefinitions[1].Width = new GridLength(imageSize + (2 * imageTouchableBorderSize), GridUnitType.Star);
                this.OuterGrid.ColumnDefinitions[2].Width = new GridLength(remaining / 2, GridUnitType.Star);

                this.Grid.RowDefinitions[0].Height = new GridLength(imageTouchableBorderSize, GridUnitType.Star);
                this.Grid.RowDefinitions[1].Height = new GridLength(imageSize, GridUnitType.Star);
                this.Grid.RowDefinitions[2].Height = new GridLength(imageTouchableBorderSize, GridUnitType.Star);

                this.Grid.ColumnDefinitions[0].Width = new GridLength(imageTouchableBorderSize, GridUnitType.Star);
                this.Grid.ColumnDefinitions[1].Width = new GridLength(imageSize, GridUnitType.Star);
                this.Grid.ColumnDefinitions[2].Width = new GridLength(imageTouchableBorderSize, GridUnitType.Star);
            }
        }
    }
}