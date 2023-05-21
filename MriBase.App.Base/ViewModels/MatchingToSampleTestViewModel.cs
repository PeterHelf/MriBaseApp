using MriBase.App.Base.Bluetooth;
using MriBase.App.Base.ExtensionMethods;
using MriBase.App.Base.Services.Interfaces;
using MriBase.Models.Models;
using MriBase.Models.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MriBase.App.Base.ViewModels
{
    public class MatchingToSampleTestViewModel : BaseTrainingViewModel
    {
        private IList<TrainingImageViewModel> currentImages;
        private IList<TrainingImage> currentSelectableImages;

        private TrainingTrial currentTrial;

        public MatchingToSampleTestViewModel(Training training, INavigationService navigationService, IOfflineChangesManager offlineChangesManager, IFeederService feederService, ILocalSaveService localSaveService, IAppDataService appDataService, IBluetoothGATTServer bluetoothGATTServer)
            : base(training, navigationService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer)
        {
        }

        public IList<TrainingImageViewModel> CurrentImages
        {
            get => currentImages;
            set
            {
                currentImages = value;
                this.OnPropertyChanged();
            }
        }

        protected override void InitNextTrial(TrainingTrial trial)
        {
            this.currentTrial = trial;
            this.CurrentImages = this.currentTrial.Parts.First().Images.Select(i => new TrainingImageViewModel(i)).ToList();

            var images = new List<TrainingImage>(trial.Parts.Last().Images);
            images.Shuffle(this.Rnd);
            this.currentSelectableImages = images;

            this.StartTimer(images);
        }

        protected override void RestartTrial()
        {
            this.CurrentImages = this.currentTrial.Parts.First().Images.Select(i => new TrainingImageViewModel(i)).ToList();
            this.StartTimer(this.currentSelectableImages);
        }

        private void StartTimer(IEnumerable<TrainingImage> images)
        {
            Task.Run(async () =>
            {
                await Task.Delay(this.Rnd.Next((int)(this.Training.SessionSettings.MinPresentationTime * 1000), (int)(this.Training.SessionSettings.MaxPresentationTime * 1000)));
                this.RemoveImages();
                await Task.Delay(Rnd.Next((int)(this.Training.SessionSettings.MinIntertrialInterval * 1000), (int)(this.Training.SessionSettings.MaxIntertrialInterval * 1000)));

                if (TimerCancellationTokenSource.Token.IsCancellationRequested)
                {
                    this.ImagesVisible = true;
                    return;
                }

                this.CurrentImages = images.Select(i => new TrainingImageViewModel(i)).ToList();
                this.ImagesVisible = true;
            }, TimerCancellationTokenSource.Token);
        }
    }
}