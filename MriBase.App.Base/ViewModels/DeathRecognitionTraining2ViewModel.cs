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
    public class DeathRecognitionTraining2ViewModel : BaseTrainingViewModel
    {
        private TrainingImageViewModel rewardImage;
        private TrainingImageViewModel continueImage;

        public TrainingImageViewModel RewardImage
        {
            get => rewardImage;
            private set
            {
                rewardImage = value;
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

        public DeathRecognitionTraining2ViewModel(Training training, INavigationService navigationService, IOfflineChangesManager offlineChangesManager, IFeederService feederService, ILocalSaveService localSaveService, IAppDataService appDataService, IBluetoothGATTServer bluetoothGATTServer)
            : base(training, navigationService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer)
        {
        }

        protected override async Task ImageClicked(TrainingImageViewModel clickedImage)
        {
            switch (clickedImage.Correctness)
            {
                case Correctness.Correct:
                    await this.CorrectAnswerGiven();
                    this.RewardImage = null;
                    break;
                case Correctness.False:
                    //await this.WrongAnswerGiven();
                    break;
                case Correctness.ExampleStimulus:
                    this.PlayContinueSound();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            await this.NextImages();
        }

        protected override List<TrainingTrial> GenerateActualTrials(List<TrainingTrial> trainingTrials, int numberOfTrials, bool randomOrder)
        {
            var actualTrials = new List<TrainingTrial>();

            for (var i = 0; i < numberOfTrials / 4 * 3; i++)
            {
                actualTrials.Add(trainingTrials[0]);
            }

            for (var i = 0; i < numberOfTrials / 4; i++)
            {
                actualTrials.Add(trainingTrials[1]);
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
            var continueInARow = 0;

            for (var i = 0; i < actualTrials.Count; i++)
            {
                if (actualTrials[i].Parts.First().Images.First().Correctness == Correctness.ExampleStimulus)
                {
                    if (continueInARow >= 2)
                    {
                        isPseudoRandom = false;
                        break;
                    }
                    else
                    {
                        continueInARow++;
                    }
                }
                else
                {
                    continueInARow = 0;
                }
            }

            return isPseudoRandom;
        }

        protected void PlayContinueSound()
        {
            PlaySound(ResAudio._350hz);
        }

        protected override void InitNextTrial(TrainingTrial trial)
        {
            var rImage = trial.Parts.First().Images.FirstOrDefault(i => i.Correctness == Correctness.Correct || i.Correctness == Correctness.False);

            if (!(rImage is null))
            {
                this.RewardImage = new TrainingImageViewModel(rImage);
            }
            else
            {
                this.RewardImage = null;
            }

            var cImage = trial.Parts.First().Images.FirstOrDefault(i => i.Correctness == Correctness.ExampleStimulus);

            if (!(cImage is null))
            {
                this.ContinueImage = new TrainingImageViewModel(cImage);
            }
            else
            {
                this.ContinueImage = null;
            }
        }
    }
}
