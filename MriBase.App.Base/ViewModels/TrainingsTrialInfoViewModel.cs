using MriBase.Models.Models;

namespace MriBase.App.Base.ViewModels
{
    public class TrainingsTrialInfoViewModel
    {
        public TrainingsTrialInfoViewModel(string name, TrainingTrial trainingsTrial)
        {
            Name = name;
            TrainingsTrial = trainingsTrial;
        }

        public TrainingTrial TrainingsTrial { get; }

        public int Length => TrainingsTrial.Parts.Count;
        public string Name { get; set; }
    }
}