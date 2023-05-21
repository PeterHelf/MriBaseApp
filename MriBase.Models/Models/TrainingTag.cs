using System;

namespace MriBase.Models.Models
{
    [Serializable]
    public class TrainingTag
    {
        public TrainingTag()
        {
            this.TagName = new TrainingTagName();
        }

        public TrainingTagName TagName { get; set; }
    }
}
