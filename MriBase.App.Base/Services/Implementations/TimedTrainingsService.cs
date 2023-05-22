using MriBase.App.Base.Services.Interfaces;
using MriBase.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace MriBase.App.Base.Services.Implementations
{
    public class TimedTrainingsService : ITimedTrainingsService
    {
        public TimedTrainingsService(ITrainingPageSelectionService trainingPageSelectionService, IAppDataService appDataService)
        {
            this.rnd = new Random();
            this.trainingPageSelectionService = trainingPageSelectionService ?? throw new ArgumentNullException(nameof(trainingPageSelectionService));
            this.appDataService = appDataService ?? throw new ArgumentNullException(nameof(appDataService));
        }

        private readonly Random rnd;
        private readonly ITrainingPageSelectionService trainingPageSelectionService;
        private readonly IAppDataService appDataService;

        private List<TimedTraining> DailyTrainings => this.appDataService.LogedInUser.DailyTrainings;

        public void StartAllTimers()
        {
            foreach (var training in DailyTrainings)
            {
                StartTraining(training);
            }
        }

        public void StartTraining(TimedTraining training) => training.StartTimer(Training);

        private async void Training(TimedTraining training)
        {
            var trainings = await this.appDataService.Trainings;

            var possibleTrainings = trainings.Where(t =>
                (training.AnyTraining || t.Id == training.SpecificTrainingId)).ToArray();

            if (!possibleTrainings.Any())
            {
                return;
            }

            appDataService.SelectedAnimal = this.appDataService.Animals.First(a => a.Id == training.AnimalId);

            int index = rnd.Next(possibleTrainings.Length);

            var selectedTraining = possibleTrainings.ElementAt(index);

            var contentPage = trainingPageSelectionService.GetTrainingPage(selectedTraining);

            Device.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage.Navigation.PushAsync(contentPage);
            });

            StartTraining(training);
        }
    }
}