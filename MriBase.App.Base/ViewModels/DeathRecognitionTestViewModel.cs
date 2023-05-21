using MriBase.App.Base.Bluetooth;
using MriBase.App.Base.ExtensionMethods;
using MriBase.App.Base.Services.Interfaces;
using MriBase.Models.Enums;
using MriBase.Models.Models;
using MriBase.Models.Resources;
using MriBase.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MriBase.App.Base.ViewModels
{
    public class DeathRecognitionTestViewModel : BaseTrainingViewModel
    {
        private TrainingImageViewModel rewardImage;
        private TrainingImageViewModel continueImage;
        private SavedSessionProgress trainingProgress;
        private bool hideBackground;

        public TrainingImageViewModel RewardImage
        {
            get => rewardImage;
            private set
            {
                rewardImage = value;
                this.OnPropertyChanged();
            }
        }

        public bool HideBackground
        {
            get => hideBackground;
            private set
            {
                hideBackground = value;
                this.OnPropertyChanged();
            }
        }

        public TrainingImageViewModel ContinueImage
        {
            get => continueImage;
            private set
            {
                continueImage = value;
                this.OnPropertyChanged();
            }
        }

        public DeathRecognitionTestViewModel(Training training, INavigationService navigationService, IOfflineChangesManager offlineChangesManager, IFeederService feederService, ILocalSaveService localSaveService, IAppDataService appDataService, IBluetoothGATTServer bluetoothGATTServer)
            : base(training, navigationService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer)
        {
        }

        protected override List<TrainingTrial> GenerateActualTrials(List<TrainingTrial> trainingTrials, int numberOfTrials, bool randomOrder)
        {
            var actualTrials = new List<TrainingTrial>();

            for (var i = 0; i < numberOfTrials / 4; i++)
            {
                actualTrials.Add(trainingTrials[0]);
            }

            for (var i = 0; i < numberOfTrials / 4 * 1.5; i++)
            {
                actualTrials.Add(trainingTrials[1]);
            }

            for (var i = 0; i < numberOfTrials / 4 * 1.5; i++)
            {
                actualTrials.Add(trainingTrials[2]);
            }

            do
            {
                actualTrials.Shuffle(this.Rnd);
            } while (!this.CheckPseudoRandomness(actualTrials));

            return actualTrials;
        }

        private bool CheckPseudoRandomness(List<TrainingTrial> actualTrials)
        {
            var isPseudoRandom = true;
            var sameInARow = 0;
            var lastId = 0;

            for (var i = 0; i < actualTrials.Count; i++)
            {
                if (actualTrials[i].Id == lastId)
                {
                    if (sameInARow >= 1)
                    {
                        isPseudoRandom = false;
                        break;
                    }
                    else
                    {
                        sameInARow++;
                    }
                }
                else
                {
                    sameInARow = 0;
                }

                lastId = actualTrials[i].Id;
            }

            return isPseudoRandom;
        }

        protected override void InitializeSession(Training training)
        {
            this.trainingProgress = this.localSaveService.LoadSessionProgress(this.Training.Id);
            if (this.trainingProgress is null)
            {
                this.trainingProgress = new SavedSessionProgress(this.Training.Id, appDataService.SelectedAnimal.Id);
            }

            var trials = new List<TrainingTrial>();
            var possibleTrials = training.TrainingTrials.Skip(1).Where(t => this.trainingProgress.FinishedTrialIds.All(u => u != t.Id)).ToList();

            trials.Add(new DeathRecognitionTrial(training.TrainingTrials.First(), -1));
            var trialNr = 0;

            if (training.SessionSettings.RandomTrialOrder)
            {
                trialNr = this.Rnd.Next(possibleTrials.Count / 2) * 2;
            }

            trials.Add(new DeathRecognitionTrial(possibleTrials[trialNr], this.Rnd.Next(5, 11)));
            trials.Add(new DeathRecognitionTrial(possibleTrials[trialNr + 1], this.Rnd.Next(5, 11)));

            this.trainingProgress.FinishedTrialIds.Add(possibleTrials[trialNr].Id);
            this.trainingProgress.FinishedTrialIds.Add(possibleTrials[trialNr].Id + 1);
            this.localSaveService.SaveSessionProgress(this.trainingProgress);

            this.ActualTrials = this.GenerateActualTrials(trials, training.SessionSettings.NumberOfTrials, training.SessionSettings.RandomTrialOrder);
        }

        protected override async Task ImageClicked(TrainingImageViewModel clickedImage)
        {
            switch (clickedImage.Correctness)
            {
                case Correctness.Correct:
                    await this.CorrectAnswerGiven();
                    break;
                case Correctness.False:
                    return;
                case Correctness.ExampleStimulus:
                    this.PlayContinueSound();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var currentTrial = this.CurrentTrial as DeathRecognitionTrial;
            currentTrial.RemainingTrials--;

            if (currentTrial.RemainingTrials == 0)
            {
                currentTrial.Parts[0].Images[0].Correctness = Correctness.False;

                for (var i = 0; i < 3; i++)
                {
                    PlayDeathSound();
                    this.ErrorScreenVisible = true;
                    await Task.Delay(200);
                    this.ErrorScreenVisible = false;
                    await Task.Delay(200);
                }
            }
            else
            {
                this.Result.EndCurrentTrial();
                await this.NextImages();
            }
        }

        protected void PlayDeathSound()
        {
            PlaySound(ResAudio._950hz);
        }

        protected void PlayContinueSound()
        {
            PlaySound(ResAudio._350hz);
        }

        protected override void InitNextTrial(TrainingTrial trial)
        {
            var rImage = trial.Parts.First().Images.First();

            if (!(rImage is null))
            {
                this.RewardImage = new TrainingImageViewModel(rImage);
            }
            else
            {
                this.RewardImage = null;
            }

            var cImage = trial.Parts.First().Images[1];

            if (!(cImage is null))
            {
                this.ContinueImage = new TrainingImageViewModel(cImage);
            }
            else
            {
                this.ContinueImage = null;
            }
        }

        protected override void EndTraining()
        {
            this.HideBackground = true;
            base.EndTraining();
        }
    }

    public class DeathRecognitionTrial : TrainingTrial
    {
        public DeathRecognitionTrial(TrainingTrial trial, int nrOfTrials)
        {
            this.Id = trial.Id;
            this.Parts = trial.Parts;
            this.BackgroundColorHexString = trial.BackgroundColorHexString;
            this.BackgroundImage = trial.BackgroundImage;

            this.RemainingTrials = nrOfTrials;
        }

        public int RemainingTrials { get; set; }
    }
}
