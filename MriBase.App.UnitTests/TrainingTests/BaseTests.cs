using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.ViewModels;
using MriBase.App.UnitTests.MockServices;
using MriBase.Models.Enums;
using MriBase.Models.Models;
using MriBase.Models.Services.Implementations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MriBase.App.UnitTests.TrainingTests
{
    internal class BaseTests
    {
        private Random rnd;
        private List<Training> trainings;
        private IContainer container;
        private MockTrainingViewModelSelectionService trainingViewModelSelectionService;

        [SetUp]
        public void Setup()
        {
            MockPlatformServices.Init();

            this.container = new MockContainer();
            this.rnd = new Random();

            var loginService = container.Resolve<ILoginService>();

            this.trainingViewModelSelectionService = container.Resolve<MockTrainingViewModelSelectionService>();

            loginService.Login("TestAdmin", PasswordService.ComputeHash("12345678"));

            var appDataService = container.Resolve<IAppDataService>();

            var trainingCreationService = container.Resolve<MockTrainingCreationService>();

            trainings = trainingCreationService.CreateDefaultTrainings();

            appDataService.SelectedAnimal = appDataService.Animals.First();

            foreach (var training in trainings)
            {
                appDataService.SelectedAnimal.Statistics.Add(new TrainingStatistic(training));
            }
        }

        [Test]
        public async Task RandomPositionTrainingTest()
        {
            var testedTrainings = 0;

            foreach (var randomPositionTraining in this.trainings.Where(t => t.TrainingType == TrainingType.RndPosTest))
            {
                var viewModel = trainingViewModelSelectionService.GetTrainingViewModel(randomPositionTraining) as RandomPositionTestViewModel;

                var totalImageClicks = 0;

                for (int i = 0; i < randomPositionTraining.SessionSettings.NumberOfTrials; totalImageClicks++)
                {
                    int index = this.rnd.Next(viewModel.CurrentImages.Count());

                    var image = viewModel.CurrentImages.ElementAt(index);
                    viewModel.ImageClickCommand.Execute(image);

                    if (image.Correctness == Correctness.Correct || !randomPositionTraining.SessionSettings.CorrectionTrialsActive)
                    {
                        i++;
                    }

                    await Task.Delay(20);
                }

                await Task.Delay(500);

                Assert.Less(viewModel.Result.SessionBegin, viewModel.Result.SessionEndTime);
                Assert.True(viewModel.Result.Trials.All(t => t.StartTime < t.EndTime));
                Assert.True(viewModel.Result.Trials.SelectMany(t => t.ClickedImages).Count() == totalImageClicks);
                Assert.True(viewModel.TrainingEnded);
                testedTrainings++;
            }

            Assert.Greater(testedTrainings, 0);
            Assert.AreEqual(this.trainings.Where(t => t.TrainingType == TrainingType.RndPosTest).Count(), testedTrainings);
        }

        [Test]
        public async Task TwoImagesTrainingTest()
        {
            var testedTrainings = 0;

            foreach (var twoImagesTraining in this.trainings.Where(t => t.TrainingType == TrainingType.TwoImgTest))
            {
                var viewModel = trainingViewModelSelectionService.GetTrainingViewModel(twoImagesTraining) as TwoImagesTestViewModel;

                var totalImageClicks = 0;

                for (int i = 0; i < twoImagesTraining.SessionSettings.NumberOfTrials; totalImageClicks++)
                {
                    TrainingImageViewModel image;

                    if (rnd.Next(2) == 0)
                    {
                        image = viewModel.RightImage;
                    }
                    else
                    {
                        image = viewModel.LeftImage;
                    }

                    viewModel.ImageClickCommand.Execute(image);

                    if (image.Correctness == Correctness.Correct || !twoImagesTraining.SessionSettings.CorrectionTrialsActive)
                    {
                        i++;
                    }

                    await Task.Delay(20);
                }

                Assert.Less(viewModel.Result.SessionBegin, viewModel.Result.SessionEndTime);
                Assert.True(viewModel.Result.Trials.All(t => t.StartTime < t.EndTime));
                Assert.True(viewModel.Result.Trials.SelectMany(t => t.ClickedImages).Count() == totalImageClicks);
                Assert.True(viewModel.TrainingEnded);
                testedTrainings++;
            }

            Assert.Greater(testedTrainings, 0);
            Assert.AreEqual(this.trainings.Where(t => t.TrainingType == TrainingType.TwoImgTest).Count(), testedTrainings);
        }

        [Test]
        public async Task MatchingToSampleTrainingTest()
        {
            var testedTrainings = 0;

            foreach (var matchingToSampleTraining in this.trainings.Where(t => t.TrainingType == TrainingType.MatchingToSample))
            {
                var viewModel = trainingViewModelSelectionService.GetTrainingViewModel(matchingToSampleTraining) as MatchingToSampleTestViewModel;

                var totalImageClicks = 0;

                for (int i = 0; i < matchingToSampleTraining.SessionSettings.NumberOfTrials; totalImageClicks++)
                {
                    int index = this.rnd.Next(viewModel.CurrentImages.Count());

                    var image = viewModel.CurrentImages.ElementAt(index);
                    viewModel.ImageClickCommand.Execute(image);

                    if (image.Correctness == Correctness.Correct || !matchingToSampleTraining.SessionSettings.CorrectionTrialsActive)
                    {
                        i++;
                    }

                    await Task.Delay(20);
                }

                Assert.Less(viewModel.Result.SessionBegin, viewModel.Result.SessionEndTime);
                Assert.True(viewModel.Result.Trials.All(t => t.StartTime < t.EndTime));
                Assert.True(viewModel.Result.Trials.SelectMany(t => t.ClickedImages).Count() == totalImageClicks);
                Assert.True(viewModel.TrainingEnded);
                testedTrainings++;
            }

            Assert.Greater(testedTrainings, 0);
            Assert.AreEqual(this.trainings.Where(t => t.TrainingType == TrainingType.MatchingToSample).Count(), testedTrainings);
        }

        [Test]
        public async Task SequentialLearningTrainingTest()
        {
            var testedTrainings = 0;

            foreach (var twoImagesTraining in this.trainings.Where(t => t.TrainingType == TrainingType.SequentialLearning))
            {
                var viewModel = trainingViewModelSelectionService.GetTrainingViewModel(twoImagesTraining) as SequentialLearningTestViewModel;

                var totalImageClicks = 0;

                for (int i = 0; i < twoImagesTraining.SessionSettings.NumberOfTrials; i++)
                {
                    await Task.Delay(20);

                    //Fehler testen
                    var wrongImage = viewModel.CurrentImages.First(i => i.Index != 0);

                    viewModel.ImageClickCommand.Execute(wrongImage);
                    totalImageClicks++;

                    await Task.Delay(20);

                    for (int j = 0; j < viewModel.CurrentImages.Count; j++)
                    {
                        var image = viewModel.CurrentImages.First(i => i.Index == j);

                        viewModel.ImageClickCommand.Execute(image);
                        totalImageClicks++;

                        await Task.Delay(20);
                    }
                }

                Assert.Less(viewModel.Result.SessionBegin, viewModel.Result.SessionEndTime);
                Assert.True(viewModel.Result.Trials.All(t => t.StartTime < t.EndTime));
                Assert.True(viewModel.Result.Trials.SelectMany(t => t.ClickedImages).Count() == totalImageClicks);
                Assert.True(viewModel.TrainingEnded);
                testedTrainings++;
            }

            Assert.Greater(testedTrainings, 0);
            Assert.AreEqual(this.trainings.Where(t => t.TrainingType == TrainingType.SequentialLearning).Count(), testedTrainings);
        }

        [Test]
        public async Task SeveralImgTrainingTest()
        {
            foreach (var severalImgTTraining in this.trainings.Where(t => t.TrainingType == TrainingType.SeveralImgTest))
            {
                var viewModel = trainingViewModelSelectionService.GetTrainingViewModel(severalImgTTraining) as SeveralImagesTestViewModel;

                var totalImageClicks = 0;

                for (int i = 0; i < severalImgTTraining.SessionSettings.NumberOfTrials; totalImageClicks++)
                {
                    int index = this.rnd.Next(viewModel.CurrentImages.Count());

                    var image = viewModel.CurrentImages.ElementAt(index);
                    viewModel.ImageClickCommand.Execute(image);

                    if (image.Correctness == Correctness.Correct || !severalImgTTraining.SessionSettings.CorrectionTrialsActive)
                    {
                        i++;
                    }

                    await Task.Delay(20);
                }

                Assert.Less(viewModel.Result.SessionBegin, viewModel.Result.SessionEndTime);
                Assert.True(viewModel.Result.Trials.All(t => t.StartTime < t.EndTime));
                Assert.True(viewModel.Result.Trials.SelectMany(t => t.ClickedImages).Count() == totalImageClicks);
                Assert.True(viewModel.TrainingEnded);
            }

            Assert.Pass();
        }

        [Test]
        public async Task SessionTimeoutTest()
        {
            var testedTrainings = 0;

            foreach (var training in this.trainings)
            {
                training.SessionSettings.MaxSessionTime = 0.5;

                var viewModel = trainingViewModelSelectionService.GetTrainingViewModel(training);

                await Task.Delay(800);

                Assert.Less(viewModel.Result.SessionBegin, viewModel.Result.SessionEndTime);
                Assert.True(viewModel.Result.Trials.Last().EndedThroughTimeout);
                Assert.True(viewModel.Result.Trials.All(t => t.StartTime < t.EndTime));
                Assert.True(viewModel.TrainingEnded);
                testedTrainings++;
            }

            Assert.Greater(testedTrainings, 0);
            Assert.AreEqual(this.trainings.Count(), testedTrainings);
        }
    }
}