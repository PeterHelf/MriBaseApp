using MriBase.App.Base.Bluetooth;
using MriBase.App.Base.ExtensionMethods;
using MriBase.App.Base.Services.Interfaces;
using MriBase.Models.Models;
using MriBase.Models.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MriBase.App.Base.ViewModels
{
    public class SeveralImagesTestViewModel : BaseTrainingViewModel
    {
        private IList<TrainingImageViewModel> currentImages;

        public IList<TrainingImageViewModel> CurrentImages
        {
            get => this.currentImages;
            set
            {
                this.currentImages = value;
                this.currentImages.Shuffle(this.Rnd);
                this.OnPropertyChanged();
            }
        }

        public SeveralImagesTestViewModel(Training training, INavigationService navigationService, IOfflineChangesManager offlineChangesManager, IFeederService feederService, ILocalSaveService localSaveService, IAppDataService appDataService, IBluetoothGATTServer bluetoothGATTServer)
            : base(training, navigationService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer)
        {
        }

        protected override void InitNextTrial(TrainingTrial trial)
        {
            var trainingImageViewModels = trial.Parts.First().Images.Select(i => new TrainingImageViewModel(i)).ToList();

            this.CurrentImages = trainingImageViewModels;
        }
    }
}
