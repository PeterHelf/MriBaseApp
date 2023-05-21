using System;

namespace MriBase.Models.Models
{
    public class TrainingStatisticDetails
    {
        public DateTime Date { get; set; }

        public int TotalErrors { get; set; }

        public double SessionDurationSeconds { get; set; }
    }
}
