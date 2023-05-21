using MriBase.App.Base.Bluetooth;
using MriBase.App.Base.Services.Interfaces;
using MriBase.Models.Enums;
using MriBase.Models.Models;
using MriBase.Models.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MriBase.App.Base.ViewModels
{
    public class GoNoGoTestViewModel : BaseTrainingViewModel
    {
        private IEnumerable<TrainingImageViewModel> currentImages;
        private TrainingTrial currentTrial;
        public bool IsInTrial { get; set; }

        private CancellationTokenSource TrialTimerCancellationTokenSource { get; set; }

        public GoNoGoTestViewModel(Training training, INavigationService navigationService, IOfflineChangesManager offlineChangesManager, IFeederService feederService, ILocalSaveService localSaveService, IAppDataService appDataService, IBluetoothGATTServer bluetoothGATTServer)
            : base(training, navigationService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer)
        {
        }

        public IEnumerable<TrainingImageViewModel> CurrentImages
        {
            get => currentImages;
            set
            {
                currentImages = value;
                this.OnPropertyChanged();
            }
        }

        protected override void RestartTrial()
        {
            this.TrialTimerCancellationTokenSource?.Cancel();
            this.TrialTimerCancellationTokenSource = new CancellationTokenSource();
            this.InitNextTrial(currentTrial);
        }

        protected override Task ImageClicked(TrainingImageViewModel clickedImage)
        {
            //this.CurrentImages = null;
            this.TrialTimerCancellationTokenSource.Cancel();
            return base.ImageClicked(clickedImage);
        }

        protected override void InitNextTrial(TrainingTrial trial)
        {
            this.currentTrial = trial;

            this.TrialTimerCancellationTokenSource?.Cancel();
            this.CurrentImages = trial.Parts.First().Images.Select(i => new TrainingImageViewModel(i));
            this.TrialTimerCancellationTokenSource = new CancellationTokenSource();
            this.StartTimer(trial.Parts.First());
        }

        protected override void EndSessionThroughTimeout()
        {
            this.TrialTimerCancellationTokenSource?.Cancel();
            base.EndSessionThroughTimeout();
        }

        private void StartTimer(TrainingTrialPart part)
        {
            var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(TimerCancellationTokenSource.Token,
                TrialTimerCancellationTokenSource.Token);
            var token = tokenSource.Token;

            Task.Run(async () =>
            {
                while (this.IsInTrial)
                {
                    await Task.Delay(20);
                }

                this.IsInTrial = true;
                await Task.Delay((int)(this.Training.SessionSettings.DecisionPhaseTime * 1000));

                if (token.IsCancellationRequested)
                {
                    this.IsInTrial = false;
                    return;
                }
                tokenSource.Dispose();

                var image = part.Images.First();
                this.CurrentImages = null;

                this.Result.TimeoutCurrentTrial();

                if (image.Correctness == Correctness.Correct)
                {
                    await this.WrongAnswerGiven();

                    if (this.Training.SessionSettings.CorrectionTrialsActive)
                    {
                        this.StartCorrectionTrial();
                    }
                }
                else
                {
                    await this.CorrectAnswerGiven();

                    this.RemoveImages();
                    await Task.Delay(Rnd.Next((int)(this.Training.SessionSettings.MinIntertrialInterval * 1000), (int)(this.Training.SessionSettings.MaxIntertrialInterval * 1000)));
                    this.ImagesVisible = true;

                    await Device.InvokeOnMainThreadAsync(async () => await this.NextImages());
                }

                this.IsInTrial = false;
            }, tokenSource.Token);
        }
    }
}