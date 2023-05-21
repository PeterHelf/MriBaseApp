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
        private int _timesPlayed;
        private int _numberOfErrors;
        private int _bestResult;

        [NonSerialized]
        private Training _training;

        public TrainingStatistic(int trainingId)
        {
            this.TrainingId = trainingId;
        }

        public TrainingStatistic(Training training)
        {
            Training = training;
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
            get => _timesPlayed;
            set
            {
                _timesPlayed = value;
                this.OnPropertyChanged();
            }
        }

        public int NumberOfErrors
        {
            get => _numberOfErrors;
            set
            {
                _numberOfErrors = value;
                this.OnPropertyChanged();
            }
        }

        public int BestResult
        {
            get => _bestResult;
            set
            {
                _bestResult = value;
                this.OnPropertyChanged();
            }
        }

        public string DurationOfTraining => this.Training is null ? null : $"~{this.Training?.SessionSettings?.NumberOfTrials * 10}";

        public Training Training
        {
            get => _training;
            set => _training = value;
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}