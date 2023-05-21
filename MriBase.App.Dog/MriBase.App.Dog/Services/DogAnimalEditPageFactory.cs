using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.ViewModels;
using MriBase.App.Base.Views;
using MriBase.App.Dog.Views;
using MriBase.Models.Interfaces;
using MriBase.Models.Services.Interfaces;
using System;

namespace MriBase.App.Dog.Services.Implementations
{
    public class DogAnimalEditPageFactory : IAnimalEditPageFactory
    {
        private readonly IAppDataService appDataService;
        private readonly INavigationService navigationService;
        private readonly IOfflineChangesManager offlineChangesManager;
        private readonly ILocalSaveService localSaveService;

        public DogAnimalEditPageFactory(IAppDataService appDataService, INavigationService navigationService, IOfflineChangesManager offlineChangesManager, ILocalSaveService localSaveService)
        {
            this.appDataService = appDataService ?? throw new ArgumentNullException(nameof(appDataService));
            this.navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            this.offlineChangesManager = offlineChangesManager ?? throw new ArgumentNullException(nameof(offlineChangesManager));
            this.localSaveService = localSaveService ?? throw new ArgumentNullException(nameof(localSaveService));
        }

        public AnimalEditPageBase CreateInstance(IAnimalInformation animalInfo)
        {
            return new AnimalEditPage(new AnimalEditViewModel(appDataService, navigationService, offlineChangesManager, animalInfo, localSaveService));
        }
    }
}
