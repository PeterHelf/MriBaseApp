using MriBase.App.Base.Bluetooth;
using MriBase.App.Base.Services.Interfaces;
using MriBase.Models.Models;
using MriBase.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MriBase.App.Base.ViewModels
{
    public class EntireTouchscreenTrainingViewModel : BaseTrainingViewModel
    {
        public Command TouchscreenClickCommand { get; }

        public EntireTouchscreenTrainingViewModel(Training training, INavigationService navigationService, IOfflineChangesManager offlineChangesManager, IFeederService feederService, ILocalSaveService localSaveService, IAppDataService appDataService, IBluetoothGATTServer bluetoothGATTServer)
               : base(training, navigationService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer)
        {
            this.TouchscreenClickCommand = new Command(async i =>
            {
                if (this.trainingEnded)
                {
                    return;
                }
                else if (i is null)
                {
                    await this.CorrectAnswerGiven();
                    await this.NextImages();
                }
                else if (i is TrainingImageViewModel clickedImage)
                {
                    this.Result.AddImageResult(this.CreateResult(clickedImage));
                    await this.ImageClicked(clickedImage);
                }
            });
        }

        public Color ScreenColor
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.CurrentTrial.BackgroundColorHexString) || Color.FromHex(this.CurrentTrial.BackgroundColorHexString) == Color.Transparent)
                {
                    return Color.FromHex(this.Training.SessionSettings.BackgroundColorHexString);
                }

                return Color.FromHex(this.CurrentTrial.BackgroundColorHexString);
            }
        }

        public new Color BackgroundColor
        {
            get
            {
                return Color.FromHex(this.Training.SessionSettings.BackgroundColorHexString);
            }
        }

        protected override void InitNextTrial(TrainingTrial trial)
        {

        }
    }
}
