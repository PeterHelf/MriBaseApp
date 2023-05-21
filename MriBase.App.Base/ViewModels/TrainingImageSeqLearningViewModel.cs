using MriBase.Models.Models;

namespace MriBase.App.Base.ViewModels
{
    public class TrainingImageSeqLearningViewModel : TrainingImageViewModel
    {
        public int Index { get; }

        public TrainingImageSeqLearningViewModel(TrainingImage trainingImage, int index) : base(trainingImage)
        {
            Index = index;
        }
    }
}