using System;
using System.Collections.Generic;
using System.Text;

namespace MriBase.Models.Models
{
    [Serializable]
    public class SavedTrialProgress
    {
        public SavedTrialProgress(int trialId, int nrOfTrials)
        {
            this.TrialId = trialId;
            this.RemainingTrials = nrOfTrials;
        }

        public int TrialId { get; set; }

        public int RemainingTrials { get; set; }
    }
}
