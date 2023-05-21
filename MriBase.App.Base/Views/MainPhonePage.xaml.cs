
using MriBase.App.Base.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MriBase.App.Base.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPhonePage : TabbedPage
    {
        private readonly MainPhoneViewModel viewModel;

        public MainPhonePage(MainPhoneViewModel viewModel)
        {
            InitializeComponent();

            this.CurrentPageChanged += ((object sender, EventArgs e) =>
            {
                viewModel.Title = this.CurrentPage.Title;
                this.popuplayout.IsVisible = false;
                this.popuplayout2.IsVisible = false;
                this.popuplayout3.IsVisible = false;
            });

            this.BindingContext = this.viewModel = viewModel;
            viewModel.Title = this.CurrentPage.Title;

            //HACK
            this.popuplayout.Animals = this.viewModel.Animals;
            this.popuplayout2.Animals = this.viewModel.Animals;
            this.popuplayout3.Animals = this.viewModel.Animals;
        }

        private async void ShowAnimalList()
        {
            AnimalListView popuplayout;

            switch (this.CurrentPage.TabIndex)
            {
                case 0:
                    popuplayout = this.popuplayout;
                    break;
                case 1:
                    popuplayout = this.popuplayout2;
                    break;
                case 2:
                    popuplayout = this.popuplayout3;
                    break;
                default:
                    throw new IndexOutOfRangeException(nameof(this.CurrentPage.TabIndex));
            }


            if (!popuplayout.IsVisible)
            {
                popuplayout.IsVisible = !popuplayout.IsVisible;
                popuplayout.AnchorX = 1;
                popuplayout.AnchorY = 0;

                Animation scaleAnimation = new Animation(
                    f => popuplayout.Scale = f,
                    0.5,
                    1,
                    Easing.SinInOut);

                Animation fadeAnimation = new Animation(
                    f => popuplayout.Opacity = f,
                    0.2,
                    1,
                    Easing.SinInOut);

                scaleAnimation.Commit(popuplayout, "popupScaleAnimation", 250);
                fadeAnimation.Commit(popuplayout, "popupFadeAnimation", 250);
            }
            else
            {
                await Task.WhenAny<bool>
                  (
                    popuplayout.FadeTo(0, 200, Easing.SinInOut)
                  );

                popuplayout.IsVisible = !popuplayout.IsVisible;
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            ShowAnimalList();
        }
    }
}