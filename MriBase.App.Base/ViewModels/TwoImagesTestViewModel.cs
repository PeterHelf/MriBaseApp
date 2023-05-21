using MriBase.App.Base.Bluetooth;
using MriBase.App.Base.Services.Interfaces;
using MriBase.Models.Models;
using MriBase.Models.Services.Interfaces;
using System;
using System.Linq;

namespace MriBase.App.Base.ViewModels
{
    public class TwoImagesTestViewModel : BaseTrainingViewModel
    {
        private readonly Random rnd = new Random();
        private TrainingImageViewModel leftImage;
        private TrainingImageViewModel rightImage;

        public TwoImagesTestViewModel(Training training, INavigationService navigationService, IOfflineChangesManager offlineChangesManager, IFeederService feederService, ILocalSaveService localSaveService, IAppDataService appDataService, IBluetoothGATTServer bluetoothGATTServer)
            : base(training, navigationService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer)
        {
        }

        public TrainingImageViewModel LeftImage
        {
            get => leftImage;
            set
            {
                leftImage = value;
                OnPropertyChanged();
            }
        }

        public TrainingImageViewModel RightImage
        {
            get => rightImage;
            set
            {
                rightImage = value;
                OnPropertyChanged();
            }
        }

        protected int LastRandomNumber { get; set; }
        protected int SameRandomNumberInARow { get; set; }

        protected override void InitNextTrial(TrainingTrial trial)
        {
            var random = rnd.Next(2);

            if (LastRandomNumber == random)
                SameRandomNumberInARow++;
            else
                SameRandomNumberInARow = 0;

            if (SameRandomNumberInARow > 1)
            {
                random = random == 1 ? 0 : 1;
                SameRandomNumberInARow = 0;
            }

            LastRandomNumber = random;

            LeftImage = new TrainingImageViewModel(trial.Parts.First().Images[random]);
            RightImage = new TrainingImageViewModel(trial.Parts.First().Images[random == 1 ? 0 : 1]);
        }
    }
}