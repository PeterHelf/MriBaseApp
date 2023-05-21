using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MriBase.App.Base.Bluetooth;
using MriBase.App.Base.Services.Implementations;
using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.ViewModels;
using MriBase.App.Base.Views;
using MriBase.App.Dog.Implementations;
using MriBase.App.Dog.ViewModels;
using MriBase.App.Dog.Views;
using MriBase.Models;
using MriBase.Models.Interfaces;
using MriBase.Models.Models;
using MriBase.Models.Services.Implementations;
using MriBase.Models.Services.Interfaces;
using System;
using System.Reflection;
using Xamarin.Forms;

namespace MriBase.App.Dog.Services.Implementations
{
    public class Container : IContainer
    {
        /// <summary>
        /// The provider for all services.
        /// </summary>
        private readonly IServiceProvider provider;

        /// <summary>
        /// Initializes static members of the <see cref="Container"/> class.
        /// </summary>
        public Container()
        {
            IServiceCollection services = new ServiceCollection();

            // Views
            services.AddTransient<AboutPage>();
            services.AddTransient<AboutPhonePage>();
            services.AddTransient<AnimalEditPageBase, AnimalEditPage>();
            services.AddTransient<AnimalRegistrationPageBase, AnimalRegistrationPage>();
            services.AddTransient<AnimalSelectionPage>();
            services.AddTransient<BluetoothSettingsPage>();
            services.AddTransient<BluetoothTrainingPage>();
            services.AddTransient<ChangePasswordPage>();
            services.AddTransient<DailyTrainingCreationPage>();
            services.AddTransient<DailyTrainingsPage>();
            services.AddTransient<FAQPage>();
            services.AddTransient<InfoPage>();
            services.AddTransient<LanguagePage>();
            services.AddTransient<LoginPage>();
            services.AddTransient<MainPhonePage>();
            services.AddTransient<PasswordForgottenPage>();
            services.AddTransient<SettingsPage>();
            services.AddTransient<StatisticsAndTrainingPage>();
            services.AddTransient<TouchscreenCalibrationPage>();
            services.AddTransient<TrainingInfoPage>();
            services.AddTransient<TrainingStatisticDetailsPage>();
            services.AddTransient<UserEditPage>();
            services.AddTransient<UserRegistrationPage>();
            services.AddTransient<VolumePage>();

            // ViewModels
            services.AddTransient<AboutPhoneViewModel>();
            services.AddTransient<AboutViewModel>();
            services.AddTransient<AnimalBreedViewModel>();
            services.AddTransient<AnimalEditViewModel>();
            services.AddTransient<AnimalInformationViewModel>();
            services.AddTransient<AnimalListViewModel>();
            services.AddTransient<AnimalRegistrationViewModelBase, AnimalRegistrationViewModel>();
            services.AddTransient<AnimalSelectionViewModelBase, AnimalSelectionViewModel>();
            services.AddTransient<BluetoothSettingsViewModel>();
            services.AddTransient<BluetoothTrainingViewModel>();
            services.AddTransient<ChangePasswordViewModel>();
            services.AddTransient<DailyTrainingCreationViewModel>();
            services.AddTransient<DailyTrainingsViewModel>();
            services.AddTransient<FAQsViewModel>();
            services.AddTransient<GroupOptionViewModel>();
            services.AddTransient<LanguageSelectionViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<MainPhoneViewModel>();
            services.AddTransient<PasswordForgottenViewModel>();
            services.AddTransient<SettingsPhoneViewModel>();
            services.AddTransient<SettingsViewModelBase, SettingsViewModel>();
            services.AddTransient<StatisticsAndTrainingViewModelBase, StatisticsAndTrainingViewModel>();
            services.AddTransient<StatisticsViewModel>();
            services.AddTransient<TimedTrainingsViewModel>();
            services.AddTransient<TrainingImageSeqLearningViewModel>();
            services.AddTransient<TrainingImageViewModel>();
            services.AddTransient<TrainingInfoViewModel>();
            services.AddTransient<TrainingSelectionViewModel>();
            services.AddTransient<TrainingsStatisticViewModel>();
            services.AddTransient<TrainingStatisticDetailsViewModel>();
            services.AddTransient<TrainingsTrialInfoViewModel>();
            services.AddTransient<TrainingViewModel>();
            services.AddTransient<UserEditViewModel>();
            services.AddTransient<UserRegistrationViewModel>();

            // Services
            services.AddSingleton<IAppDataService, AppDataService>();
            services.AddSingleton<IConfigService, ConfigService>();
            services.AddSingleton<ILocalSaveService, DogLocalSaveService>();
            services.AddSingleton<ILoginService, DogLoginService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IOfflineChangesManager, OfflineChangesManager>();
            services.AddSingleton<ITimedTrainingsService, TimedTrainingsService>();
            services.AddSingleton<ITouchscreenCalibrationService, TouchscreenCalibrationService>();
            services.AddSingleton<ITrainingPageSelectionService, TrainingPageSelectionService>();
            services.AddSingleton<IImageRecourceService, DogImageRecourceService>();

            // Factories
            services.AddSingleton(typeof(IFactory<AnimalEditPageBase, IAnimalInformation>), typeof(DogAnimalEditPageFactory));
            services.AddSingleton(typeof(IFactory<BluetoothTrainingPage, Training>), typeof(BluetoothTrainingPageFactory));
            services.AddSingleton(typeof(IFactory<TrainingInfoPage, Training>), typeof(TrainingInfoPageFactory));
            services.AddSingleton(typeof(IFactory<TrainingStatisticDetailsPage, TrainingStatistic>), typeof(TrainingStatisticDetailsPageFactory));
            //services.AddSingleton<IAnimalEditPageFactory, AnimalEditPageFactory>();
            //services.AddSingleton<IBluetoothTrainingPageFactory, BluetoothTrainingPageFactory>();
            //services.AddSingleton<ITrainingInfoPageFactory, TrainingInfoPageFactory>();
            //services.AddSingleton<ITrainingStatisticDetailsPageFactory, TrainingStatisticDetailsPageFactory>();

            // Bluetooth Services
            services.AddSingleton<IBluetoothGATTServer, BluetoothGATTServer>();
            services.AddSingleton<IBluetoothTrainingService, BluetoothTrainingService>();

            // Config
            var configFileName = GetType().GetTypeInfo().Assembly.GetName().Name + ".appsettings.json";
            var resourceStream = GetType().GetTypeInfo().Assembly.GetManifestResourceStream(configFileName);
            var configuration = new ConfigurationBuilder()
                .AddJsonStream(resourceStream)
                .Build();
            services.AddSingleton<IConfiguration>(configuration);

            var useBluetooth = bool.Parse(configuration.GetSection("Bluetooth")["Enabled"]);
            if (useBluetooth)
            {
                services.AddSingleton<IFeederService, BluetoothService>();
                services.AddSingleton<IBluetoothService, BluetoothService>(c => (BluetoothService)c.GetService<IFeederService>());
            }
            else
            {
                services.AddSingleton<IFeederService, USBFeederService>();
                services.AddSingleton<IBluetoothService, BluetoothService>();
            }

            services.AddSingleton<IContainer, Container>(sp => this);

            // Logging
            services.AddLogging();

            provider = services.BuildServiceProvider();
        }

        /// <summary>
        /// Gets an item out of the service provider.
        /// </summary>
        /// <typeparam name="TItem">The type of the wanted item.</typeparam>
        /// <returns>The wanted item.</returns>
        public TItem Resolve<TItem>()
        {
            return provider.GetService<TItem>();
        }
    }
}
