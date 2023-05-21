using MriBase.App.Base.Bluetooth;
using MriBase.App.Base.Services.Interfaces;
using MriBase.Models.Models;
using MriBase.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MriBase.App.Base.ViewModels
{
    public class SingleImageTrainingViewModel : BaseTrainingViewModel
    {
        private TrainingImageViewModel image;

        public TrainingImageViewModel Image
        {
            get => image; 
            
            set
            {
                image = value;
                this.OnPropertyChanged();
            }
        }

        public SingleImageTrainingViewModel(Training training, INavigationService navigationService,IOfflineChangesManager offlineChangesManager, IFeederService feederService, ILocalSaveService localSaveService, IAppDataService appDataService, IBluetoothGATTServer bluetoothGATTServer)
            : base(training, navigationService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer)
        {
        }

        protected override void InitNextTrial(TrainingTrial trial)
        {
            var trainingImageViewModels = trial.Parts.First().Images.Select(i => new TrainingImageViewModel(i)).ToList();

            this.Image = trainingImageViewModels.First();
        }
    }
}
