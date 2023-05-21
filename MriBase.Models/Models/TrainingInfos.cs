using MriBase.Models.Enums;
using System;
using System.Collections.Generic;

namespace MriBase.Models.Models
{
    public class TrainingInfos
    {
        public TrainingName TrainingName { get; set; }

        public List<TrainingTag> Tags { get; set; }

        public string Description { get; set; }

        public byte[] ThumbnailImage { get; set; }

        public TrainingType TrainingType { get; set; }

        public DateTime LastChange { get; set; }

        public int Id { get; set; }

        public bool Active { get; set; }
    }
}
