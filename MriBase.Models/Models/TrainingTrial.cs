using System;
using System.Collections.Generic;

namespace MriBase.Models.Models
{
    [Serializable]
    public class TrainingTrial
    {
        public int Id { get; set; }

        public List<TrainingTrialPart> Parts { get; set; }

        public string BackgroundColorHexString { get; set; }

        public byte[] BackgroundImage { get; set; }

        public TrainingTrial()
        {
            this.Parts = new List<TrainingTrialPart>();
        }
    }
}