using MriBase.App.Base.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MriBase.App.Base.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnimalSelectionPage : ContentPage
    {
        private readonly AnimalSelectionViewModelBase viewModel;

        public AnimalSelectionPage(AnimalSelectionViewModelBase viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;

            UpdateAnimalList();
            this.viewModel.PropertyChanged += AnimalListChanged;
        }

        private void AnimalListChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.viewModel.AnimalViewModels)) UpdateAnimalList();
        }

        private void UpdateAnimalList()
        {
            var addStackLayout = new StackLayout();
            var addImage = new Image();
            var addLabel = new Label();
            var gestRecog = new TapGestureRecognizer();

            addImage.Source = this.viewModel.AddAnimalImage;
            addImage.HorizontalOptions = LayoutOptions.Center;
            addImage.HeightRequest = 200;
            addImage.WidthRequest = 200;
            addImage.Aspect = Aspect.AspectFill;

            addLabel.HorizontalOptions = LayoutOptions.Center;
            addLabel.SetBinding(Label.TextProperty, new Binding
            {
                Mode = BindingMode.OneWay,
                Path = $"AnimalAddButtonText",
                Source = viewModel
            });

            gestRecog.Command = this.viewModel.AddAnimalCommand;
            gestRecog.NumberOfTapsRequired = 1;

            addStackLayout.HorizontalOptions = LayoutOptions.Center;
            addStackLayout.Children.Add(addImage);
            addStackLayout.Children.Add(addLabel);
            addStackLayout.GestureRecognizers.Add(gestRecog);
            addStackLayout.Margin = new Thickness(5);
            addStackLayout.HeightRequest = 230;

            AnimalList.Children.Add(addStackLayout);
        }
    }
}