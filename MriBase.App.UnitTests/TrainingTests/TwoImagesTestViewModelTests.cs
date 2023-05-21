using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.ViewModels;
using MriBase.App.UnitTests.MockServices;
using MriBase.App.UnitTests.TestTrainings;
using MriBase.Models.Enums;
using MriBase.Models.Models;
using MriBase.Models.Services.Implementations;
using NUnit.Framework;
using System.Linq;

namespace MriBase.App.UnitTests.TrainingTests
{
    internal class TwoImagesTestViewModelTests
    {
        private MockContainer container;
        private MockTrainingViewModelSelectionService trainingViewModelSelectionService;
        private Training training;
        private TestableTwoImagesTestViewModel trainingViewModel;

        [SetUp]
        public void Setup()
        {
            MockPlatformServices.Init();

            this.container = new MockContainer();

            var loginService = container.Resolve<ILoginService>();

            this.trainingViewModelSelectionService = container.Resolve<MockTrainingViewModelSelectionService>();

            loginService.Login("TestAdmin", PasswordService.ComputeHash("12345678"));

            var appDataService = container.Resolve<IAppDataService>();

            var trainingCreationService = container.Resolve<MockTrainingCreationService>();

            this.training = trainingCreationService.CreateDefaultTwoImagesTraining();

            appDataService.SelectedAnimal = appDataService.Animals.First();

            appDataService.SelectedAnimal.Statistics.Add(new TrainingStatistic(training));

            this.trainingViewModel = trainingViewModelSelectionService.GetTrainingViewModel(training) as TestableTwoImagesTestViewModel;
        }

        [Test]
        public void CorrectImageClickTest()
        {
            TrainingImageViewModel image;

            if (this.trainingViewModel.LeftImage.Correctness == Correctness.Correct)
            {
                image = this.trainingViewModel.LeftImage;
            }
            else if (this.trainingViewModel.RightImage.Correctness == Correctness.Correct)
            {
                image = this.trainingViewModel.RightImage;
            }
            else
            {
                Assert.Fail();
                return;
            }

            this.trainingViewModel.ImageClickCommand.Execute(image);

            var firstResult = this.trainingViewModel.Result.Trials.First();

            Assert.IsFalse(firstResult.EndedThroughTimeout);
            Assert.IsFalse(firstResult.IsCorrectionTrial);
            Assert.IsTrue(firstResult.IsCorrect);
            Assert.AreEqual(firstResult.ClickedImages.Count, 1);
            Assert.AreEqual(firstResult.ClickedImages.First().Correctness, Correctness.Correct);
        }

        [Test]
        public void FalseImageClickTest()
        {
            TrainingImageViewModel image;

            if (this.trainingViewModel.LeftImage.Correctness == Correctness.False)
            {
                image = this.trainingViewModel.LeftImage;
            }
            else if (this.trainingViewModel.RightImage.Correctness == Correctness.False)
            {
                image = this.trainingViewModel.RightImage;
            }
            else
            {
                Assert.Fail();
                return;
            }

            this.trainingViewModel.ImageClickCommand.Execute(image);

            var firstResult = this.trainingViewModel.Result.Trials.First();

            Assert.IsFalse(firstResult.EndedThroughTimeout);
            Assert.IsFalse(firstResult.IsCorrectionTrial);
            Assert.IsFalse(firstResult.IsCorrect);
            Assert.AreEqual(firstResult.ClickedImages.Count, 1);
            Assert.AreEqual(firstResult.ClickedImages.First().Correctness, Correctness.False);
        }

        [Test]
        public void CreatedTrialsTest()
        {
            Assert.AreEqual(this.trainingViewModel.ActualTrials.Count, this.training.SessionSettings.NumberOfTrials);
        }

        [Repeat(20)]
        [Test]
        public void ImagesOnSameSideLeftTest()
        {
            trainingViewModel.LastRandomNumber = 0;
            trainingViewModel.SameRandomNumberInARow = 2;

            var trial = trainingViewModel.ActualTrials[trainingViewModel.CurrentTrialIndex];
            this.trainingViewModel.InitNextTrial(trial);

            Assert.AreEqual(trainingViewModel.RightImage.TrainingsImage, trial.Parts.First().Images[0]);
            Assert.AreEqual(trainingViewModel.LeftImage.TrainingsImage, trial.Parts.First().Images[1]);
        }

        [Repeat(20)]
        [Test]
        public void ImagesOnSameSideRightTest()
        {
            trainingViewModel.LastRandomNumber = 1;
            trainingViewModel.SameRandomNumberInARow = 2;

            var trial = trainingViewModel.ActualTrials[trainingViewModel.CurrentTrialIndex];
            this.trainingViewModel.InitNextTrial(trial);

            Assert.AreEqual(trainingViewModel.RightImage.TrainingsImage, trial.Parts.First().Images[1]);
            Assert.AreEqual(trainingViewModel.LeftImage.TrainingsImage, trial.Parts.First().Images[0]);
        }
    }
}
