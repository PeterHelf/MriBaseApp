using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MriBase.Models.Models
{
    [Serializable]
    public class TrainingTrialResult
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int TrialId { get; set; }

        public List<TrainingClickedImageResult> ClickedImages { get; set; }

        public bool IsCorrectionTrial { get; set; }

        public bool EndedThroughTimeout { get; set; }

        public bool IsCorrect { get; set; }

        public TrainingTrialResult(int trialId, bool isCorrectionTrial)
        {
            this.StartTime = DateTime.UtcNow;
            this.TrialId = trialId;
            this.ClickedImages = new List<TrainingClickedImageResult>();
            this.IsCorrectionTrial = isCorrectionTrial;
        }

        /// <summary>
        /// Leerer Constructor für JSON Deserialisierung
        /// </summary>
        [JsonConstructor]
        private TrainingTrialResult()
        {
            this.ClickedImages = new List<TrainingClickedImageResult>();
        }
    }
}