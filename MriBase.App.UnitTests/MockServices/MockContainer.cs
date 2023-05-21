using Microsoft.Extensions.DependencyInjection;
using MriBase.App.Base.Bluetooth;
using MriBase.App.Base.Services.Interfaces;
using MriBase.Models.Services.Interfaces;
using System;

namespace MriBase.App.UnitTests.MockServices
{
    internal class MockContainer : IContainer
    {
        /// <summary>
        /// The provider for all services.
        /// </summary>
        private readonly IServiceProvider provider;

        /// <summary>
        /// Initializes static members of the <see cref="Container"/> class.
        /// </summary>
        public MockContainer()
        {
            IServiceCollection services = new ServiceCollection();

            // Services
            services.AddSingleton<IAppDataService, MockAppDataService>();
            services.AddSingleton<ILocalSaveService, MockAppLocalSaveService>();
            services.AddSingleton<ILoginService, MockAppLoginService>();
            services.AddSingleton<MockTrainingViewModelSelectionService>();
            services.AddSingleton<IRestService, MockRestService>();
            services.AddSingleton<IBluetoothService, MockBluetoothService>();
            services.AddSingleton<IFeederService, MockFeederService>();
            services.AddSingleton<MockTrainingCreationService>();

            services.AddSingleton<IContainer, MockContainer>(sp => this);

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
