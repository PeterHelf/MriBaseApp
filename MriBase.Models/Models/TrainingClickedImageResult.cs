using MriBase.Models.Enums;
using Newtonsoft.Json;
using System;

namespace MriBase.Models.Models
{
    [Serializable]
    public class TrainingClickedImageResult
    {
        public int ClickedImageId { get; set; }

        public DateTime TimeOfClick { get; set; }

        public Correctness Correctness { get; set; }

        /// <summary>
        /// Leerer Constructor für JSON Deserialisierung
        /// </summary>
        [JsonConstructor]
        private TrainingClickedImageResult()
        {
        }

        public TrainingClickedImageResult(TrainingImage image)
        {
            this.TimeOfClick = DateTime.UtcNow;
            this.ClickedImageId = image.Id;
            this.Correctness = image.Correctness;
        }

        public TrainingClickedImageResult(int imageId, Correctness correctness)
        {
            this.TimeOfClick = DateTime.UtcNow;
            this.ClickedImageId = imageId;
            this.Correctness = correctness;
        }
    }
}