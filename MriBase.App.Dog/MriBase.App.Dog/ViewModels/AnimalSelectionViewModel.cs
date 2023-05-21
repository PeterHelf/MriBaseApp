using MriBase.App.Base.Bluetooth;
using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.ViewModels;
using MriBase.Models.Resources;
using MriBase.Models.Services.Interfaces;

namespace MriBase.App.Dog.ViewModels
{
    public class AnimalSelectionViewModel : AnimalSelectionViewModelBase
    {
        public AnimalSelectionViewModel(INavigationService navigationService, IBluetoothGATTServer bluetoothGATTServer, ILocalSaveService localSaveService, IAppDataService appDataService, IImageRecourceService imageRecourceService) : base(navigationService, bluetoothGATTServer, localSaveService, appDataService, imageRecourceService)
        {
        }

        public override string AnimalListHeaderText => ResViewAnimalSelection.WhichAnimal;

        public override string AnimalAddButtonText => ResViewAnimalSelection.AddAnimal;
    }
}