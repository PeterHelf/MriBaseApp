using MriBase.Models.Enums;
using MriBase.Models.Models;
using System;

namespace MriBase.App.Base.ViewModels
{
    public class TimedTrainingsViewModel
    {
        public TimedTrainingsViewModel(TimedTraining timedTraining)
        {
            TimedTraining = timedTraining;
        }

        public TimedTraining TimedTraining { get; }

        public string StartTime => new DateTime().Add(this.TimedTraining.StartTime).ToShortTimeString();
        public string MinDuration => this.TimedTraining.MinDuration.ToString();
        public string MaxDuration => this.TimedTraining.MaxDuration.ToString();
        public string AnimalName => this.TimedTraining.Animal.Name;
        public bool AnyTraining => this.TimedTraining.AnyTraining;
        public TrainingType TrainingType => this.TimedTraining.SpecificTrainingType;
    }
}