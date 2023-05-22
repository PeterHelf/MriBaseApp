using MriBase.Models.Translation;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MriBase.Models.Models
{
    [Serializable]
    public class TrainingStatistic : INotifyPropertyChanged
    {
        private int timesPlayed;
        private int numberOfErrors;
        private int bestResult;

        [NonSerialized]
        private Training training;

        public TrainingStatistic(int trainingId)
        {
            this.TrainingId = trainingId;
        }

        public TrainingStatistic(Training training)
        {
            this.Training = training;
            this.TrainingId = training.Id;
        }

        /// <summary>
        /// Leerer Constructor für JSON Deserialisierung
        /// </summary>
        [JsonConstructor]
        private TrainingStatistic()
        {
        }

        public int TrainingId { get; set; }

        [JsonIgnore]
        public string Name => Translator.Instance.TranslateText(this.Training?.Name);

        public int TimesPlayed
        {
            get => this.timesPlayed;
            set
            {
                this.timesPlayed = value;
                this.OnPropertyChanged();
            }
        }

        public int NumberOfErrors
        {
            get => this.numberOfErrors;
            set
            {
                this.numberOfErrors = value;
                this.OnPropertyChanged();
            }
        }

        public int BestResult
        {
            get => this.bestResult;
            set
            {
                this.bestResult = value;
                this.OnPropertyChanged();
            }
        }

        public Training Training
        {
            get => this.training;
            set => this.training = value;
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}