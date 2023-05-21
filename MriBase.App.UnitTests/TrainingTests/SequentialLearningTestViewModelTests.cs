using MriBase.App.Base.Services.Interfaces;
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
    internal class SequentialLearningTestViewModelTests
    {
        private MockContainer container;
        private MockTrainingViewModelSelectionService trainingViewModelSelectionService;
        private Training training;
        private TestableSequentialLearningTestViewModel trainingViewModel;

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

            this.training = trainingCreationService.CreateDefaultSequentialLearningTraining();

            appDataService.SelectedAnimal = appDataService.Animals.First();

            appDataService.SelectedAnimal.Statistics.Add(new TrainingStatistic(training));

            this.trainingViewModel = trainingViewModelSelectionService.GetTrainingViewModel(training) as TestableSequentialLearningTestViewModel;
        }

        [Test]
        public void CorrectImageClickTest()
        {
            Thread.Sleep(100);
            var images = this.trainingViewModel.CurrentImages.ToList();

            var orderedImages = images.OrderBy(i => i.Index);

            foreach (var image in orderedImages)
            {
                this.trainingViewModel.ImageClickCommand.Execute(image);
            }

            var firstResult = this.trainingViewModel.Result.Trials.First();

            Assert.IsFalse(firstResult.EndedThroughTimeout);
            Assert.IsFalse(firstResult.IsCorrectionTrial);
            Assert.IsTrue(firstResult.IsCorrect);
            Assert.AreEqual(firstResult.ClickedImages.Count, trainingViewModel.CurrentImages.Count);
            Assert.IsTrue(firstResult.ClickedImages.All(c => c.Correctness == Correctness.Correct));
        }

        [Test]
        public void FalseImageClickTest()
        {
            Thread.Sleep(100);
            var images = this.trainingViewModel.CurrentImages.ToList();

            var orderedImages = images.OrderBy(i => i.Index).Reverse();

            this.trainingViewModel.ImageClickCommand.Execute(orderedImages.First());

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
    }
}
