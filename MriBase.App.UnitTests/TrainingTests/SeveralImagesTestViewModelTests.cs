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
    internal class SeveralImagesTestViewModelTests
    {
        private MockContainer container;
        private MockTrainingViewModelSelectionService trainingViewModelSelectionService;
        private Training training;
        private TestableSeveralImagesTestViewModel trainingViewModel;

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

            this.training = trainingCreationService.CreateDefaultServeralImagesTraining();

            appDataService.SelectedAnimal = appDataService.Animals.First();

            appDataService.SelectedAnimal.Statistics.Add(new TrainingStatistic(training));

            this.trainingViewModel = trainingViewModelSelectionService.GetTrainingViewModel(training) as TestableSeveralImagesTestViewModel;
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
    }
}
