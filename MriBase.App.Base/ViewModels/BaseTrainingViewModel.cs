using MriBase.App.Base.Bluetooth;
using MriBase.App.Base.ExtensionMethods;
using MriBase.App.Base.Services.Interfaces;
using MriBase.Models.Enums;
using MriBase.Models.EventArgs;
using MriBase.Models.Models;
using MriBase.Models.Resources;
using MriBase.Models.Services.Interfaces;
using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MriBase.App.Base.ViewModels
{
    public abstract class BaseTrainingViewModel : BaseViewModel
    {
        protected Random Rnd { get; set; }
        protected Training Training { get; set; }
        private readonly INavigationService navigationService;
        private readonly IOfflineChangesManager offlineChangesManager;
        private readonly IFeederService feederService;
        protected readonly ILocalSaveService localSaveService;
        protected readonly IAppDataService appDataService;
        private readonly IBluetoothGATTServer bluetoothGATTServer;
        private bool imagesVisible;
        protected bool trainingEnded;
        private bool errorScreenVisible;
        private int currentTrialIndex;

        public TrainingSessionResult Result { get; }
        public CancellationTokenSource TimerCancellationTokenSource { get; set; }
        public int CurrentTrialIndex
        {
            get => currentTrialIndex;
            set
            {
                currentTrialIndex = value;
                this.OnPropertyChanged(nameof(this.BackgroundColor));
                this.OnPropertyChanged(nameof(this.BackgroundImage));
            }
        }
        public virtual TrainingTrial CurrentTrial => this.ActualTrials[this.CurrentTrialIndex];
        public Command ImageClickCommand { get; set; }
        public Command ReturnToPreviousPageCommand { get; set; }
        public bool BroadcastResultWithBluetooth { get; set; }

        public Color BackgroundColor
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

        public ImageSource BackgroundImage
        {
            get
            {
                if (this.CurrentTrial.BackgroundImage is null)
                {
                    if (this.Training.SessionSettings.BackgroundImage is null)
                    {
                        return null;
                    }

                    return ImageSource.FromStream(() => new MemoryStream(this.Training.SessionSettings.BackgroundImage));
                }

                return ImageSource.FromStream(() => new MemoryStream(this.CurrentTrial.BackgroundImage));
            }
        }

        protected List<TrainingTrial> ActualTrials { get; set; }

        public bool ImagesVisible
        {
            get => imagesVisible;
            set
            {
                imagesVisible = value;
                this.OnPropertyChanged();
            }
        }

        public bool TrainingEnded
        {
            get
            {
                return trainingEnded;
            }

            set
            {
                trainingEnded = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.SwipeEnabled));
            }
        }

        public bool SwipeEnabled
        {
            get
            {
                if (Device.RuntimePlatform == Device.UWP)
                {
                    return true;
                }

                return TrainingEnded;
            }
        }

        public bool ErrorScreenVisible
        {
            get => errorScreenVisible;
            set
            {
                errorScreenVisible = value;
                this.OnPropertyChanged();
            }
        }

        public bool FeederReady { get; private set; }

        protected BaseTrainingViewModel(Training training, INavigationService navigationService, IOfflineChangesManager offlineChangesManager, IFeederService bluetoothService, ILocalSaveService localSaveService, IAppDataService appDataService, IBluetoothGATTServer bluetoothGATTServer)
        {
            this.Rnd = new Random();
            this.Training = training;
            this.navigationService = navigationService;
            this.offlineChangesManager = offlineChangesManager;
            this.feederService = bluetoothService;
            this.localSaveService = localSaveService;
            this.appDataService = appDataService;
            this.bluetoothGATTServer = bluetoothGATTServer;
            this.TimerCancellationTokenSource = new CancellationTokenSource();
            this.ImagesVisible = true;
            this.Result = new TrainingSessionResult(training.Id, this.appDataService.SelectedAnimal.Id, Device.RuntimePlatform, DeviceInfo.Model, DeviceDisplay.MainDisplayInfo.Height, DeviceDisplay.MainDisplayInfo.Width);
            this.currentTrialIndex = -1;

            this.FeederReady = true;
            this.feederService.BytesReceived += FeederBytesReceived;

            InitializeSession(training);

            if (this.Training.SessionSettings.MaxSessionTime > 0)
            {
                EndSessionThroughTimeout();
            }

            this.ImageClickCommand = new Command(async i =>
            {
                if (i is TrainingImageViewModel clickedImage)
                {
                    this.Result.AddImageResult(this.CreateResult(clickedImage));
                    await this.ImageClicked(clickedImage);
                }
            });

            this.ReturnToPreviousPageCommand = new Command(async () =>
            {
                await this.navigationService.ReturnToLastPage();
            });

            this.StartFirstTrial();
        }

        protected virtual void InitializeSession(Training training)
        {
            this.ActualTrials = this.GenerateActualTrials(training.TrainingTrials, training.SessionSettings.NumberOfTrials, training.SessionSettings.RandomTrialOrder);
        }

        protected virtual void EndSessionThroughTimeout()
        {
            Task.Run(async () =>
            {
                await Task.Delay((int)(this.Training.SessionSettings.MaxSessionTime * 1000));

                if (TimerCancellationTokenSource.Token.IsCancellationRequested)
                {
                    return;
                }

                this.Result.TimeoutCurrentTrial();
                Device.BeginInvokeOnMainThread(() => this.EndTraining());
            }, TimerCancellationTokenSource.Token);
        }

        protected virtual List<TrainingTrial> GenerateActualTrials(List<TrainingTrial> trainingTrials, int numberOfTrials, bool randomOrder)
        {
            var actualTrials = new List<TrainingTrial>();

            for (int i = 0; i < numberOfTrials; i++)
            {
                actualTrials.Add(trainingTrials[i % trainingTrials.Count]);
            }

            if (randomOrder)
            {
                actualTrials.Shuffle(this.Rnd);
            }

            return actualTrials;
        }

        private void FeederBytesReceived(object sender, BytesReceivedEventArgs e)
        {
            try
            {
                var message = UTF8Encoding.UTF8.GetString(e.Bytes);

                if (message == "4")
                {
                    this.FeederReady = true;
                }
            }
            catch (Exception)
            {
            }
        }

        public void StopSession()
        {
            if (!this.trainingEnded)
            {
                this.Result.SessionCanceled = true;
                EndTraining();
            }
        }

        protected virtual void EndTraining()
        {
            this.TimerCancellationTokenSource.Cancel();
            this.Result.SessionEndTime = DateTime.UtcNow;
            this.TrainingEnded = true;
            this.RemoveImages();
            this.PlayTrainingEndSound();
            this.SaveResults();
        }

        protected void PlaySuccessSound()
        {
            PlaySound(ResAudio._600hz);
        }

        protected void PlayFailureSound()
        {
            PlaySound(ResAudio._200hz);
        }

        protected void PlayTrainingStartSound()
        {
            PlaySound(ResAudio.TrainingStart);
        }

        protected void PlayTrainingEndSound()
        {
            PlaySound(ResAudio.TrainingEnd);
        }

        protected void PlaySound(Stream soundStream)
        {
            try
            {
                var audio = CrossSimpleAudioPlayer.Current;
                audio.Load(soundStream);
                audio.Volume = this.appDataService.UserSettings.Volume;
                audio.Play();
            }
            catch (Exception)
            {
            }
        }

        protected virtual async Task ImageClicked(TrainingImageViewModel clickedImage)
        {
            switch (clickedImage.Correctness)
            {
                case Correctness.Correct:
                    await this.CorrectAnswerGiven();
                    break;
                case Correctness.False:
                    await this.WrongAnswerGiven();
                    break;
                case Correctness.ExampleStimulus:
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (clickedImage.Correctness == Correctness.False && this.Training.SessionSettings.CorrectionTrialsActive)
            {
                this.StartCorrectionTrial();
                return;
            }

            await this.NextImages();
        }

        protected async Task StartNextTrial()
        {
            if (this.trainingEnded)
            {
                return;
            }

            this.RemoveImages();
            await Task.Delay(Rnd.Next((int)(this.Training.SessionSettings.MinIntertrialInterval * 1000), (int)(this.Training.SessionSettings.MaxIntertrialInterval * 1000)));

            //if (BluetoothManager.FeederDeviceConnected)
            //{
            //    await Task.Run(async () =>
            //    {
            //        while (!this.FeederReady || !BluetoothManager.FeederDeviceConnected)
            //        {
            //            await Task.Delay(50);
            //        }
            //    });
            //}

            var nextTrial = GetNextTrial();

            this.Result.StartNewTrial(nextTrial, false);
            this.InitNextTrial(nextTrial);
            if (Device.RuntimePlatform == Device.UWP && this.Training.TrainingType == TrainingType.TwoImgTest)
            {
                await Task.Delay(10);
            }
            this.ImagesVisible = true;
        }

        protected virtual TrainingTrial GetNextTrial()
        {
            this.CurrentTrialIndex++;
            return this.CurrentTrial;
        }

        private void StartFirstTrial()
        {
            if (this.feederService.FeederDeviceConnected)
            {
                this.feederService.FillFood();
            }

            this.PlayTrainingStartSound();

            var nextTrial = GetNextTrial();
            this.Result.StartNewTrial(nextTrial, false);
            this.InitNextTrial(nextTrial);
        }

        protected virtual TrainingClickedImageResult CreateResult(TrainingImageViewModel clickedImage)
        {
            return new TrainingClickedImageResult(clickedImage.TrainingsImage);
        }

        protected void RemoveImages()
        {
            this.ImagesVisible = false;
        }

        //TODO:CorrectionTrials erweitern
        protected async void StartCorrectionTrial()
        {
            this.RemoveImages();
            await Task.Delay(Rnd.Next((int)(this.Training.SessionSettings.MinCorrectionTrialIntertrialInterval * 1000), (int)(this.Training.SessionSettings.MaxCorrectionTrialIntertrialInterval * 1000)));

            this.Result.StartNewTrial(CurrentTrial, true);
            this.RestartTrial();
            this.ImagesVisible = true;
        }

        protected async Task NextImages()
        {
            if (CheckIfTrainingEnded())
            {
                this.EndTraining();
                return;
            }

            await this.StartNextTrial();

            if (this.feederService.FeederDeviceConnected)
            {
                this.feederService.FillFood();
            }
        }

        protected virtual bool CheckIfTrainingEnded()
        {
            return CurrentTrialIndex + 1 >= this.Training.SessionSettings.NumberOfTrials;
        }

        protected virtual void RestartTrial()
        {

        }

        protected abstract void InitNextTrial(TrainingTrial trial);

        protected async Task WrongAnswerGiven()
        {
            this.Result.EndCurrentTrial();
            this.Result.MarkTrialAsWrong();

            if (this.BroadcastResultWithBluetooth)
            {
                bluetoothGATTServer?.BroadcastTrainingTrialResult(this.Result.Trials.LastOrDefault());
            }

            PlayFailureSound();
            this.ErrorScreenVisible = true;
            await Task.Delay((int)(this.Training.SessionSettings.TimeOutAfterWrongChoice * 1000));
            this.ErrorScreenVisible = false;
        }

        protected async Task CorrectAnswerGiven()
        {
            this.Result.EndCurrentTrial();
            this.Result.MarkTrialAsCorrect();

            if (this.feederService.FeederDeviceConnected)
            {
                await this.feederService.GiveOutFood();
                this.FeederReady = false;
            }

            if (this.BroadcastResultWithBluetooth)
            {
                bluetoothGATTServer?.BroadcastTrainingTrialResult(this.Result.Trials.LastOrDefault());
            }

            PlaySuccessSound();
        }

        protected async void SaveResults()
        {
            var stats = this.appDataService.SelectedAnimal.Statistics.First(s => s.Training == this.Training);

            var totalErrors = this.Result.Trials.Count(r => !r.IsCorrect);

            stats.TimesPlayed++;
            stats.NumberOfErrors += totalErrors;

            if (stats.BestResult == 0 || stats.BestResult > totalErrors)
            {
                stats.BestResult = totalErrors;
            }

            await this.localSaveService.SaveAnimals();

            if (this.BroadcastResultWithBluetooth)
            {
                bluetoothGATTServer?.BroadcastTrainingResult(this.Result);
            }

            await Task.Delay(2000);

            this.offlineChangesManager.AddResult(this.Result);
        }
    }
}