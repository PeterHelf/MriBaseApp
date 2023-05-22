using MriBase.App.Base.Services.Implementations;
using MriBase.App.Base.Services.Interfaces;
using MriBase.Models.Enums;
using MriBase.Models.Models;
using System;
using System.Linq;

namespace MriBase.App.Base.ViewModels
{
    public class TimedTrainingsViewModel
    {
        private readonly IAppDataService appDataService;

        public TimedTrainingsViewModel(TimedTraining timedTraining, IAppDataService appDataService)
        {
            TimedTraining = timedTraining;
            this.appDataService = appDataService;
        }

        public TimedTraining TimedTraining { get; }

        public string StartTime => new DateTime().Add(this.TimedTraining.StartTime).ToShortTimeString();
        public string AnimalName => this.appDataService.Animals.First(a => a.Id == TimedTraining.AnimalId).Name;
        public bool AnyTraining => this.TimedTraining.AnyTraining;
        public string TrainingName => this.TimedTraining.SpecificTrainingId == 0 ? string.Empty : this.appDataService.Trainings.Result.First(t => t.Id == this.TimedTraining.SpecificTrainingId).Name.ToString();
    }
}