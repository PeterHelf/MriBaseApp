using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MriBase.Models.Models
{
    [Serializable]
    public class TrainingSessionResult
    {
        public DateTime SessionBegin { get; set; }

        public DateTime SessionEndTime { get; set; }

        public int TrainingId { get; set; }

        public int AnimalId { get; set; }

        public bool SessionCanceled { get; set; }

        public List<TrainingTrialResult> Trials { get; set; }

        public FraudDetectionData FraudDetectionData { get; set; }

        public TrainingSessionResult(int trainingId, int animalId, string deviceType, string deviceModel, double deviceHeight, double deviceWidth)
        {
            this.AnimalId = animalId;
            this.SessionBegin = DateTime.UtcNow;
            this.TrainingId = trainingId;
            this.Trials = new List<TrainingTrialResult>();
            this.FraudDetectionData = new FraudDetectionData(deviceType, deviceModel, deviceHeight, deviceWidth);
        }

        /// <summary>
        /// Leerer Constructor für JSON Deserialisierung
        /// </summary>
        [JsonConstructor]
        private TrainingSessionResult()
        {
            this.Trials = new List<TrainingTrialResult>();
        }

        public void MarkTrialAsCorrect()
        {
            var lastTrial = this.Trials.LastOrDefault();
            if (!(lastTrial is null))
            {
                lastTrial.IsCorrect = true;
            }
        }

        public void MarkTrialAsWrong()
        {
            var lastTrial = this.Trials.LastOrDefault();
            if (!(lastTrial is null))
            {
                lastTrial.IsCorrect = false;
            }
        }

        public void AddImageResult(TrainingClickedImageResult image)
        {
            this.Trials.LastOrDefault()?.ClickedImages.Add(image);
        }

        public void StartNewTrial(TrainingTrial trial, bool isCorrectionTrial)
        {
            this.Trials.Add(new TrainingTrialResult(trial.Id, isCorrectionTrial));
        }

        public void EndCurrentTrial()
        {
            var lastTrial = this.Trials.LastOrDefault();
            if (!(lastTrial is null))
            {
                lastTrial.EndTime = DateTime.UtcNow;
            }
        }

        public void TimeoutCurrentTrial()
        {
            var lastTrial = this.Trials.LastOrDefault();
            if (!(lastTrial is null))
            {
                lastTrial.EndTime = DateTime.UtcNow;
                lastTrial.EndedThroughTimeout = true;
            }
        }
    }
}