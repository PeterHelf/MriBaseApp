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
    public class DeathRecognitionTraining1ViewModel : BaseTrainingViewModel
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

        public DeathRecognitionTraining1ViewModel(Training training, INavigationService navigationService, IOfflineChangesManager offlineChangesManager, IFeederService feederService, ILocalSaveService localSaveService, IAppDataService appDataService, IBluetoothGATTServer bluetoothGATTServer)
            : base(training, navigationService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer)
        {
        }

        protected override List<TrainingTrial> GenerateActualTrials(List<TrainingTrial> trainingTrials, int numberOfTrials, bool randomOrder)
        {
            if (!randomOrder)
            {
                return base.GenerateActualTrials(trainingTrials, numberOfTrials, randomOrder);
            }

            var actualTrials = base.GenerateActualTrials(trainingTrials, numberOfTrials, randomOrder);

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
            var rewardInARow = 0;

            for (var i = 0; i < actualTrials.Count; i++)
            {
                if (actualTrials[i].Parts.First().Images.First().Correctness == Correctness.ExampleStimulus)
                {
                    rewardInARow = 0;
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
                    if (rewardInARow >= 2)
                    {
                        isPseudoRandom = false;
                        break;
                    }
                    else
                    {
                        rewardInARow++;
                    }
                }
            }

            return isPseudoRandom;
        }

        protected override async Task ImageClicked(TrainingImageViewModel clickedImage)
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
                    PlayContinueSound();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            await this.NextImages();
        }

        protected void PlayContinueSound()
        {
            PlaySound(ResAudio._350hz);
        }

        protected override void InitNextTrial(TrainingTrial trial)
        {
            if (trial.Parts.First().Images.First().Correctness == Correctness.Correct)
            {
                this.RewardImage = new TrainingImageViewModel(trial.Parts.First().Images.First());
                this.ContinueImage = null;
            }
            else
            {
                this.RewardImage = null;
                this.ContinueImage = new TrainingImageViewModel(trial.Parts.First().Images.First());
            }
        }
    }
}
