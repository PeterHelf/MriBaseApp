using System;
using System.Collections.Generic;

namespace MriBase.Models.Models
{
    [Serializable]
    public class SavedSessionProgress
    {
        public int TrainingId { get; set; }

        public int AnimalId { get; set; }

        public List<int> FinishedTrialIds { get; set; }

        public SavedSessionProgress(int trainingId, int animalId)
        {
            this.TrainingId = trainingId;
            this.AnimalId = animalId;

            this.FinishedTrialIds = new List<int>();
        }
    }
}
