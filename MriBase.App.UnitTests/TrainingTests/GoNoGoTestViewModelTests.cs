using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.ViewModels;
using MriBase.App.UnitTests.MockServices;
using MriBase.App.UnitTests.TestTrainings;
using MriBase.Models.Enums;
using MriBase.Models.Models;
using MriBase.Models.Services.Implementations;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;

namespace MriBase.App.UnitTests.TrainingTests
{
    internal class GoNoGoTestViewModelTests
    {
        private MockContainer container;
        private MockTrainingViewModelSelectionService trainingViewModelSelectionService;
        private Training training;
        private TestableGoNoGoTestViewModel trainingViewModel;

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

            this.training = trainingCreationService.CreateDefaultGoNoGoTraining();

            appDataService.SelectedAnimal = appDataService.Animals.First();

            appDataService.SelectedAnimal.Statistics.Add(new TrainingStatistic(training));

            this.trainingViewModel = trainingViewModelSelectionService.GetTrainingViewModel(training) as TestableGoNoGoTestViewModel;
        }

        [Test]
        public void CorrectImageClickTest()
        {
            var image = new TrainingImageViewModel(new TrainingImage() { Correctness = Correctness.Correct });

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
            var image = new TrainingImageViewModel(new TrainingImage() { Correctness = Correctness.False });

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

        [Repeat(10)]
        [Test]
        public void TimeoutTrialTest()
        {
            var image = this.trainingViewModel.CurrentImages.First();

            Thread.Sleep((int)Math.Round(training.SessionSettings.DecisionPhaseTime * 1200, MidpointRounding.ToPositiveInfinity));

            var firstResult = this.trainingViewModel.Result.Trials.First();

            Assert.IsTrue(firstResult.EndedThroughTimeout);
            Assert.IsFalse(firstResult.IsCorrectionTrial);
            Assert.Less(firstResult.StartTime, firstResult.EndTime);
            Assert.AreEqual(firstResult.ClickedImages.Count, 0);

            if (image.TrainingsImage.Correctness == Correctness.Correct)
            {
                Assert.IsFalse(firstResult.IsCorrect);
            }
            else
            {
                Assert.IsTrue(firstResult.IsCorrect);
            }
        }
    }
}
