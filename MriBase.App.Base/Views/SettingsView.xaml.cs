using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MriBase.App.Base.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsView : ContentView
    {
        public static readonly BindableProperty AboutButtonCommandProperty =
            BindableProperty.Create(nameof(AboutButtonCommand), typeof(ICommand), typeof(SettingsView));

        public static readonly BindableProperty LanguageButtonCommandProperty =
            BindableProperty.Create(nameof(LanguageButtonCommand), typeof(ICommand), typeof(SettingsView));

        public static readonly BindableProperty VolumeButtonCommandProperty =
            BindableProperty.Create(nameof(VolumeButtonCommand), typeof(ICommand), typeof(SettingsView));

        public static readonly BindableProperty DailyTrainingsButtonCommandProperty =
            BindableProperty.Create(nameof(DailyTrainingsButtonCommand), typeof(ICommand), typeof(SettingsView));

        public static readonly BindableProperty BluetoothSettingsButtonCommandProperty =
            BindableProperty.Create(nameof(BluetoothSettingsButtonCommand), typeof(ICommand), typeof(SettingsView));

        public static readonly BindableProperty TouchscreenCalibrationButtonCommandProperty =
            BindableProperty.Create(nameof(TouchscreenCalibrationButtonCommand), typeof(ICommand), typeof(SettingsView));

        public SettingsView()
        {
            InitializeComponent();
        }

        public ICommand AboutButtonCommand
        {
            get => (ICommand)GetValue(AboutButtonCommandProperty);
            set => SetValue(AboutButtonCommandProperty, value);
        }

        public ICommand LanguageButtonCommand
        {
            get => (ICommand)GetValue(LanguageButtonCommandProperty);
            set => SetValue(LanguageButtonCommandProperty, value);
        }

        public ICommand VolumeButtonCommand
        {
            get => (ICommand)GetValue(VolumeButtonCommandProperty);
            set => SetValue(VolumeButtonCommandProperty, value);
        }

        public ICommand DailyTrainingsButtonCommand
        {
            get => (ICommand)GetValue(DailyTrainingsButtonCommandProperty);
            set => SetValue(DailyTrainingsButtonCommandProperty, value);
        }

        public ICommand BluetoothSettingsButtonCommand
        {
            get => (ICommand)GetValue(BluetoothSettingsButtonCommandProperty);
            set => SetValue(BluetoothSettingsButtonCommandProperty, value);
        }

        public ICommand TouchscreenCalibrationButtonCommand
        {
            get => (ICommand)GetValue(TouchscreenCalibrationButtonCommandProperty);
            set => SetValue(TouchscreenCalibrationButtonCommandProperty, value);
        }        
    }
}