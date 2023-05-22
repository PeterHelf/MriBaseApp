using MriBase.App.Base.Interfaces;
using MriBase.App.Base.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MriBase.App.Base.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [ContentProperty("Content")]
    public partial class BaseTrainingPage : ContentPage
    {
        public BaseTrainingViewModel ViewModel { get; set; }

        public BaseTrainingPage(BaseTrainingViewModel viewModel)
        {
            InitializeComponent();

            this.BindingContext = this.ViewModel = viewModel;
        }

        protected override void OnAppearing()
        {
            DependencyService.Get<IStatusBar>().HideStatusBar();
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            DependencyService.Get<IStatusBar>().ShowStatusBar();
            this.ViewModel.TimerCancellationTokenSource.Cancel();
            this.ViewModel.StopSession();
            base.OnDisappearing();
        }

        public View TrainingContent
        {
            get { return this.ContentView.Content; }
            set { this.ContentView.Content = value; }
        }

        public void KeyPressed(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.ViewModel.ReturnToPreviousPageCommand.Execute(null);
                    break;
                case Key.Left:
                    if (e.ShiftDown)
                    {
                        this.Grid.Margin = new Thickness(this.Grid.Margin.Left - 1, this.Grid.Margin.Top, this.Grid.Margin.Right, this.Grid.Margin.Bottom);
                    }
                    else if (e.ControlDown)
                    {
                        this.Grid.Margin = new Thickness(this.Grid.Margin.Left, this.Grid.Margin.Top, this.Grid.Margin.Right + 1, this.Grid.Margin.Bottom);
                    }
                    else
                    {
                        this.Grid.Margin = new Thickness(this.Grid.Margin.Left - 1, this.Grid.Margin.Top, this.Grid.Margin.Right + 1, this.Grid.Margin.Bottom);
                    }
                    break;
                case Key.Up:
                    if (e.ShiftDown)
                    {
                        this.Grid.Margin = new Thickness(this.Grid.Margin.Left, this.Grid.Margin.Top - 1, this.Grid.Margin.Right, this.Grid.Margin.Bottom);
                    }
                    else if (e.ControlDown)
                    {
                        this.Grid.Margin = new Thickness(this.Grid.Margin.Left, this.Grid.Margin.Top, this.Grid.Margin.Right, this.Grid.Margin.Bottom + 1);
                    }
                    else
                    {
                        this.Grid.Margin = new Thickness(this.Grid.Margin.Left, this.Grid.Margin.Top - 1, this.Grid.Margin.Right, this.Grid.Margin.Bottom + 1);
                    }
                    break;
                case Key.Right:
                    if (e.ShiftDown)
                    {
                        this.Grid.Margin = new Thickness(this.Grid.Margin.Left, this.Grid.Margin.Top, this.Grid.Margin.Right - 1, this.Grid.Margin.Bottom);
                    }
                    else if (e.ControlDown)
                    {
                        this.Grid.Margin = new Thickness(this.Grid.Margin.Left + 1, this.Grid.Margin.Top, this.Grid.Margin.Right, this.Grid.Margin.Bottom);
                    }
                    else
                    {
                        this.Grid.Margin = new Thickness(this.Grid.Margin.Left + 1, this.Grid.Margin.Top, this.Grid.Margin.Right - 1, this.Grid.Margin.Bottom);
                    }
                    break;
                case Key.Down:
                    if (e.ShiftDown)
                    {
                        this.Grid.Margin = new Thickness(this.Grid.Margin.Left, this.Grid.Margin.Top, this.Grid.Margin.Right, this.Grid.Margin.Bottom - 1);
                    }
                    else if (e.ControlDown)
                    {
                        this.Grid.Margin = new Thickness(this.Grid.Margin.Left, this.Grid.Margin.Top + 1, this.Grid.Margin.Right, this.Grid.Margin.Bottom);
                    }
                    else
                    {
                        this.Grid.Margin = new Thickness(this.Grid.Margin.Left, this.Grid.Margin.Top + 1, this.Grid.Margin.Right, this.Grid.Margin.Bottom - 1);
                    }
                    break;
            }
        }
    }

    public class KeyEventArgs : EventArgs
    {
        public KeyEventArgs(Key key, bool shiftDown = false, bool controlDown = false)
        {
            this.Key = key;
            this.ShiftDown = shiftDown;
            this.ControlDown = controlDown;
        }

        public bool ShiftDown { get; }

        public bool ControlDown { get; }

        public Key Key { get; }
    }

    public enum Key
    {
        Escape,
        Left,
        Up,
        Right,
        Down,
    }
}