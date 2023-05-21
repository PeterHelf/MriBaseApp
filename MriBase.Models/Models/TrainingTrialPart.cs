using System;
using System.Collections.Generic;

namespace MriBase.Models.Models
{
    [Serializable]
    public class TrainingTrialPart
    {
        public int Id { get; set; }

        public List<TrainingImage> Images { get; set; }

        public TrainingTrialPart()
        {
            this.Images = new List<TrainingImage>();
        }
    }
}