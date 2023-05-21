using MriBase.App.Base.Bluetooth;
using MriBase.App.Base.ExtensionMethods;
using MriBase.App.Base.Services.Interfaces;
using MriBase.Models.Enums;
using MriBase.Models.Models;
using MriBase.Models.Services.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MriBase.App.Base.ViewModels
{
    public class SequentialLearningTestViewModel : BaseTrainingViewModel
    {
        private ObservableCollection<TrainingImageSeqLearningViewModel> currentImages;

        public SequentialLearningTestViewModel(Training training, INavigationService navigationService, IOfflineChangesManager offlineChangesManager, IFeederService feederService, ILocalSaveService localSaveService, IAppDataService appDataService, IBluetoothGATTServer bluetoothGATTServer)
            : base(training, navigationService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer)
        {
        }



        public List<TrainingImageSeqLearningViewModel> ImagesVersionA { get; set; }

        public List<TrainingImageSeqLearningViewModel> ImagesVersionB { get; set; }

        private int currentSequenceIndex;

        private bool inExamplePhase;

        protected override TrainingClickedImageResult CreateResult(TrainingImageViewModel clickedImage)
        {
            if (this.inExamplePhase)
            {
                return new TrainingClickedImageResult(clickedImage.TrainingsImage.Id, Correctness.ExampleStimulus);
            }

            if (clickedImage is TrainingImageSeqLearningViewModel seqImage && seqImage.Index == currentSequenceIndex)
            {
                return new TrainingClickedImageResult(clickedImage.TrainingsImage.Id, Correctness.Correct);
            }

            return new TrainingClickedImageResult(clickedImage.TrainingsImage.Id, Correctness.False);
        }

        public ObservableCollection<TrainingImageSeqLearningViewModel> CurrentImages
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
            this.inExamplePhase = true;
            this.currentSequenceIndex = 0;

            this.CurrentImages = ImagesVersionA.ToObservableCollection();
            this.StartTimer();
        }

        protected override async Task ImageClicked(TrainingImageViewModel clickedImage)
        {
            if (inExamplePhase)
            {
                return;
            }

            if (clickedImage is TrainingImageSeqLearningViewModel seqImage && seqImage.Index == currentSequenceIndex)
            {
                var image = CurrentImages.First(imageVm => imageVm.Index == currentSequenceIndex);
                var index = CurrentImages.IndexOf(image);

                await Device.InvokeOnMainThreadAsync(() => CurrentImages[index] = ImagesVersionB[index]);
                this.currentSequenceIndex++;

                if (this.currentSequenceIndex >= this.CurrentImages.Count)
                {
                    await this.CorrectAnswerGiven();

                    await this.NextImages();
                }
            }
            else
            {
                await this.WrongAnswerGiven();

                if (this.Training.SessionSettings.CorrectionTrialsActive)
                {
                    this.StartCorrectionTrial();
                    return;
                }

                await this.NextImages();
            }
        }

        protected override void InitNextTrial(TrainingTrial trial)
        {
            this.ImagesVersionA = trial.Parts.First().Images.Select((i, index) => new TrainingImageSeqLearningViewModel(new TrainingImage() { Correctness = Correctness.ExampleStimulus, Image = i.Image, Id = i.Id }, index)).ToList();
            this.ImagesVersionB = trial.Parts.Last().Images.Select((i, index) => new TrainingImageSeqLearningViewModel(new TrainingImage() { Correctness = Correctness.ExampleStimulus, Image = i.Image, Id = i.Id }, index)).ToList();
            this.inExamplePhase = true;
            this.currentSequenceIndex = 0;
            this.ShuffleTwoLists(this.ImagesVersionA, this.ImagesVersionB);

            this.CurrentImages = ImagesVersionA.ToObservableCollection();
            this.StartTimer();
        }

        private void ShuffleTwoLists(IList<TrainingImageSeqLearningViewModel> list1, IList<TrainingImageSeqLearningViewModel> list2)
        {
            for (var i = list1.Count - 1; i >= 1; --i)
            {
                var j = this.Rnd.Next(i + 1);
                list1.Swap(i, j);
                list2.Swap(i, j);
            }
        }

        private void StartTimer()
        {
            Task.Run(async () =>
            {
                for (var i = 0; i < CurrentImages.Count(); i++)
                {
                    await Task.Delay(this.Rnd.Next((int)(this.Training.SessionSettings.MinPresentationTime * 1000), (int)(this.Training.SessionSettings.MaxPresentationTime * 1000)));

                    if (TimerCancellationTokenSource.IsCancellationRequested)
                    {
                        return;
                    }

                    var image = CurrentImages.First(imageVm => imageVm.Index == i);
                    var index = CurrentImages.IndexOf(image);

                    await Device.InvokeOnMainThreadAsync(() => CurrentImages[index] = ImagesVersionB[index]);
                }

                await Task.Delay(this.Rnd.Next((int)(this.Training.SessionSettings.MinPresentationTime * 1000), (int)(this.Training.SessionSettings.MaxPresentationTime * 1000)));
                this.CurrentImages = ImagesVersionA.ToObservableCollection();
                this.inExamplePhase = false;
            }, TimerCancellationTokenSource.Token);
        }
    }
}