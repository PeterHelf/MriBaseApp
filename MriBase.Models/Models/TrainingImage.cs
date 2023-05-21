using MriBase.Models.Enums;
using System;

namespace MriBase.Models.Models
{
    [Serializable]
    public class TrainingImage
    {
        public TrainingImage()
        {
            this.ImageSize = 1;
            this.ImageTouchableBorderSize = 0.1;
        }

        public int Id { get; set; }

        public byte[] Image { get; set; }

        public Correctness Correctness { get; set; }

        public double ImageSize { get; set; }

        public double ImageTouchableBorderSize { get; set; }
    }
}