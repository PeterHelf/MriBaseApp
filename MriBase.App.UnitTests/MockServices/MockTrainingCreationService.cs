using MriBase.Models.Enums;
using MriBase.Models.Models;
using System.Collections.Generic;

namespace MriBase.App.UnitTests.MockServices
{
    public class MockTrainingCreationService
    {
        public List<Training> CreateDefaultTrainings()
        {
            var trainings = new List<Training>();

            var randomPostionTraining = this.CreateDefaultRandomPostionTraining();

            trainings.Add(randomPostionTraining);

            var twoImagesTraining = this.CreateDefaultTwoImagesTraining();

            trainings.Add(twoImagesTraining);

            var matchingToSampleTraining = this.CreateDefaultMatchinToSampleTraining();

            trainings.Add(matchingToSampleTraining);

            var sequentialLearningTraining = CreateDefaultSequentialLearningTraining();

            trainings.Add(sequentialLearningTraining);

            var severalImgTraining = CreateDefaultServeralImagesTraining();

            trainings.Add(severalImgTraining);

            return trainings;
        }

        public Training CreateDefaultGoNoGoTraining()
        {
            var goNoGoTraining = new Training(null, TrainingType.GoNoGo);
            goNoGoTraining.Name["en"] = "goNoGoTest1";
            goNoGoTraining.Id = 1;
            goNoGoTraining.SessionSettings = this.CreateDefaultSessionSettigns();
            goNoGoTraining.SessionSettings.DecisionPhaseTime = 150d / 1000;
            goNoGoTraining.SessionSettings.MinIntertrialInterval = 50d / 1000;
            goNoGoTraining.SessionSettings.MaxIntertrialInterval = 50d / 1000;

            var trainingTrial2 = new TrainingTrial();

            var trialPart2 = new TrainingTrialPart();

            var trainingImage3 = new TrainingImage()
            {
                Correctness = Correctness.Correct
            };

            trialPart2.Images.Add(trainingImage3);

            trainingTrial2.Parts.Add(trialPart2);

            var trainingTrial3 = new TrainingTrial();

            var trialPart3 = new TrainingTrialPart();

            var trainingImage4 = new TrainingImage()
            {
                Correctness = Correctness.False
            };

            trialPart3.Images.Add(trainingImage4);

            trainingTrial3.Parts.Add(trialPart3);

            goNoGoTraining.TrainingTrials.Add(trainingTrial2);
            goNoGoTraining.TrainingTrials.Add(trainingTrial3);

            return goNoGoTraining;
        }

        public Training CreateDefaultServeralImagesTraining()
        {
            var severalImgTraining = new Training(null, TrainingType.SeveralImgTest);
            severalImgTraining.Name["en"] = "severalImgTest1";
            severalImgTraining.Id = 1;
            severalImgTraining.SessionSettings = this.CreateDefaultSessionSettigns();

            var trainingTrial7 = new TrainingTrial();

            var trialPart9 = new TrainingTrialPart();

            var trainingImage16 = new TrainingImage()
            {
                Correctness = Correctness.False
            };

            var trainingImage17 = new TrainingImage()
            {
                Correctness = Correctness.Correct
            };

            var trainingImage18 = new TrainingImage()
            {
                Correctness = Correctness.False
            };

            trialPart9.Images.Add(trainingImage16);
            trialPart9.Images.Add(trainingImage17);
            trialPart9.Images.Add(trainingImage18);

            trainingTrial7.Parts.Add(trialPart9);

            severalImgTraining.TrainingTrials.Add(trainingTrial7);
            return severalImgTraining;
        }

        public Training CreateDefaultSequentialLearningTraining()
        {
            var sequentialLearningTraining = new Training(null, TrainingType.SequentialLearning);
            sequentialLearningTraining.Name["en"] = "SeqTest1";
            sequentialLearningTraining.Id = 1;
            sequentialLearningTraining.SessionSettings = this.CreateDefaultSessionSettigns();

            var trainingTrial6 = new TrainingTrial();

            var trialPart7 = new TrainingTrialPart();

            var trainingImage10 = new TrainingImage()
            {
                Correctness = Correctness.ExampleStimulus
            };

            var trainingImage11 = new TrainingImage()
            {
                Correctness = Correctness.ExampleStimulus
            };

            var trainingImage12 = new TrainingImage()
            {
                Correctness = Correctness.ExampleStimulus
            };

            trialPart7.Images.Add(trainingImage10);
            trialPart7.Images.Add(trainingImage11);
            trialPart7.Images.Add(trainingImage12);

            var trialPart8 = new TrainingTrialPart();

            var trainingImage13 = new TrainingImage()
            {
                Correctness = Correctness.False
            };

            var trainingImage14 = new TrainingImage()
            {
                Correctness = Correctness.False
            };

            var trainingImage15 = new TrainingImage()
            {
                Correctness = Correctness.False
            };

            trialPart8.Images.Add(trainingImage13);
            trialPart8.Images.Add(trainingImage14);
            trialPart8.Images.Add(trainingImage15);

            trainingTrial6.Parts.Add(trialPart7);
            trainingTrial6.Parts.Add(trialPart8);

            sequentialLearningTraining.TrainingTrials.Add(trainingTrial6);
            return sequentialLearningTraining;
        }

        public Training CreateDefaultMatchinToSampleTraining()
        {
            var matchingToSampleTraining = new Training(null, TrainingType.MatchingToSample);
            matchingToSampleTraining.Name["en"] = "matchingTest1";
            matchingToSampleTraining.Id = 1;
            matchingToSampleTraining.SessionSettings = this.CreateDefaultSessionSettigns();

            var trainingTrial5 = new TrainingTrial();

            var trialPart5 = new TrainingTrialPart();

            var trainingImage7 = new TrainingImage()
            {
                Correctness = Correctness.ExampleStimulus
            };

            trialPart5.Images.Add(trainingImage7);

            var trialPart6 = new TrainingTrialPart();

            var trainingImage8 = new TrainingImage()
            {
                Correctness = Correctness.Correct
            };

            var trainingImage9 = new TrainingImage()
            {
                Correctness = Correctness.False
            };

            trialPart6.Images.Add(trainingImage8);
            trialPart6.Images.Add(trainingImage9);

            trainingTrial5.Parts.Add(trialPart5);
            trainingTrial5.Parts.Add(trialPart6);

            matchingToSampleTraining.TrainingTrials.Add(trainingTrial5);
            return matchingToSampleTraining;
        }

        public Training CreateDefaultTwoImagesTraining()
        {
            var twoImagesTraining = new Training(null, TrainingType.TwoImgTest);
            twoImagesTraining.Name["en"] = "twoImgTest1";
            twoImagesTraining.Id = 1;
            twoImagesTraining.SessionSettings = this.CreateDefaultSessionSettigns();

            var trainingTrial4 = new TrainingTrial();

            var trialPart4 = new TrainingTrialPart();

            var trainingImage5 = new TrainingImage()
            {
                Correctness = Correctness.Correct
            };

            var trainingImage6 = new TrainingImage()
            {
                Correctness = Correctness.False
            };

            trialPart4.Images.Add(trainingImage5);
            trialPart4.Images.Add(trainingImage6);

            trainingTrial4.Parts.Add(trialPart4);

            twoImagesTraining.TrainingTrials.Add(trainingTrial4);
            return twoImagesTraining;
        }

        public Training CreateDefaultRandomPostionTraining()
        {
            var randomPostionTraining = new Training(null, TrainingType.RndPosTest);
            randomPostionTraining.Name["en"] = "RndTest1";
            randomPostionTraining.SessionSettings = this.CreateDefaultSessionSettigns();

            var trainingTrial = new TrainingTrial();

            var trialPart = new TrainingTrialPart();

            var trainingImage1 = new TrainingImage();

            var trainingImage2 = new TrainingImage();

            trialPart.Images.Add(trainingImage1);
            trialPart.Images.Add(trainingImage2);

            trainingTrial.Parts.Add(trialPart);

            randomPostionTraining.TrainingTrials.Add(trainingTrial);

            return randomPostionTraining;
        }

        private SessionSettings CreateDefaultSessionSettigns()
        {
            return new SessionSettings
            {
                MinIntertrialInterval = 0,
                MaxIntertrialInterval = 0,
                ClickFreeIntertrialInterval = 0,
                NumberOfTrials = 10,
                MaxSessionTime = 0,
                MinPresentationTime = 0,
                MaxPresentationTime = 0,
                DecisionPhaseTime = 0,
                NeededClicks = 0,
                CorrectionTrialsActive = true,
                RandomTrialOrder = true,
                BackgroundColorHexString = "#000000",
                MinCorrectionTrialIntertrialInterval = 0,
                MaxCorrectionTrialIntertrialInterval = 0,
                TimeOutAfterWrongChoice = 0,
                BackgroundImage = null
            };
        }
    }
}
