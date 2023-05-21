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
    public class SingleImageMultiplePositionsTrainingViewModel : BaseTrainingViewModel
    {
        private readonly Random rnd = new Random();
        private TrainingImageViewModel image;
        private bool imageInMiddle;
        private bool imageLeft;
        private bool imageRight;

        public TrainingImageViewModel Image
        {
            get => image;

            set
            {
                image = value;
                this.OnPropertyChanged();
            }
        }

        public bool ImageInMiddle
        {
            get => imageInMiddle;

            private set
            {
                imageInMiddle = value;
                this.OnPropertyChanged();
            }
        }

        public bool ImageLeft
        {
            get => imageLeft;

            private set
            {
                imageLeft = value;
                this.OnPropertyChanged();
            }
        }

        public bool ImageRight
        {
            get => imageRight; 
            
            private set
            {
                imageRight = value;
                this.OnPropertyChanged();
            }
        }

        public int LastRandomNumber { get; private set; }
        public int SameRandomNumberInARow { get; private set; }

        public SingleImageMultiplePositionsTrainingViewModel(Training training, INavigationService navigationService, IOfflineChangesManager offlineChangesManager, IFeederService feederService, ILocalSaveService localSaveService, IAppDataService appDataService, IBluetoothGATTServer bluetoothGATTServer)
            : base(training, navigationService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer)
        {
        }

        protected override void InitNextTrial(TrainingTrial trial)
        {
            if (this.CurrentTrialIndex % 2 == 0)
            {
                this.ImageInMiddle = true;
                this.ImageLeft = false;
                this.ImageRight = false;
            }
            else
            {
                this.ImageInMiddle = false;

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

                this.ImageLeft = random != 1;
                this.ImageRight = random == 1;
            }

            var trainingImageViewModels = trial.Parts.First().Images.Select(i => new TrainingImageViewModel(i)).ToList();

            this.Image = trainingImageViewModels.First();
        }
    }
}
