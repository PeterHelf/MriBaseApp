using System;

namespace MriBase.Models.Models
{
    [Serializable]
    public class SessionSettings
    {
        public double MinIntertrialInterval { get; set; }

        public double MaxIntertrialInterval { get; set; }

        public double ClickFreeIntertrialInterval { get; set; }

        public int NumberOfTrials { get; set; }

        public double MaxSessionTime { get; set; }

        public double MinPresentationTime { get; set; }

        public double MaxPresentationTime { get; set; }

        public double DecisionPhaseTime { get; set; }

        public int NeededClicks { get; set; }

        public bool CorrectionTrialsActive { get; set; }

        public bool RandomTrialOrder { get; set; }

        public string BackgroundColorHexString { get; set; }

        public double MinCorrectionTrialIntertrialInterval { get; set; }

        public double MaxCorrectionTrialIntertrialInterval { get; set; }

        public double TimeOutAfterWrongChoice { get; set; }

        public byte[] BackgroundImage { get; set; }
    }
}
