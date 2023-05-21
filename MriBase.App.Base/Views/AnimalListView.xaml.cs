using MriBase.App.Base.ViewModels;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MriBase.App.Base.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnimalListView : ContentView
    {
        public static readonly BindableProperty AnimalsProperty =
            BindableProperty.Create(nameof(Animals), typeof(IEnumerable<AnimalInformationViewModel>), typeof(AnimalListView), propertyChanged: OnAnimalsPropertyChanged);

        public IEnumerable<AnimalInformationViewModel> Animals
        {
            get { return base.GetValue(AnimalsProperty) as IEnumerable<AnimalInformationViewModel>; }
            set { base.SetValue(AnimalsProperty, value); }
        }

        private readonly AnimalListViewModel viewModel;

        public AnimalListView()
        {
            InitializeComponent();

            this.BindingContext = this.viewModel = BaseViewModel.Container.Resolve<AnimalListViewModel>();
        }

        static void OnAnimalsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((AnimalListView)bindable).OnAnimalsPropertyChanged(newValue as IEnumerable<AnimalInformationViewModel>);
        }

        void OnAnimalsPropertyChanged(IEnumerable<AnimalInformationViewModel> newValue)
        {
            this.viewModel.Animals = newValue;
        }
    }
}