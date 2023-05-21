using MriBase.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MriBase.Models.Models
{
    [Serializable]
    public class Training
    {
        public Training(TrainingName name, string description, IEnumerable<TrainingTrial> trials, byte[] image, TrainingType trainingType)
        {
            Name = name;
            Description = description;
            Image = image;
            TrainingType = trainingType;

            this.TrainingTrials = new List<TrainingTrial>(trials);
            this.SessionSettings = new SessionSettings();
            this.Tags = new List<TrainingTag>();
        }

        public Training(byte[] image, TrainingType trainingType)
        {
            Name = new TrainingName() { Translations = new List<Translation>(new[] { new Translation("en", string.Empty) }) };
            Image = image;
            TrainingType = trainingType;

            this.TrainingTrials = new List<TrainingTrial>();
            this.SessionSettings = new SessionSettings();
            this.Tags = new List<TrainingTag>();
        }

        /// <summary>
        /// Leerer Constructor für JSON Deserialisierung
        /// </summary>
        [JsonConstructor]
        private Training()
        {
            this.TrainingTrials = new List<TrainingTrial>();
            this.Tags = new List<TrainingTag>();
            this.SessionSettings = new SessionSettings();
        }

        public int Id { get; set; }

        public TrainingName Name { get; set; }

        public string Description { get; set; }

        public List<TrainingTrial> TrainingTrials { get; set; }

        public List<TrainingTag> Tags { get; set; }

        public byte[] Image { get; set; }

        public TrainingType TrainingType { get; set; }

        public SessionSettings SessionSettings { get; set; }

        public DateTime LastChange { get; set; }
    }
}