using MriBase.Models.Enums;
using MriBase.Models.Models;
using MriBase.Models.Translation;
using System.Linq;

namespace MriBase.App.Base.ViewModels
{
    public class TrainingsStatisticViewModel : BaseViewModel
    {
        public TrainingStatistic Statistic { get; set; }

        public string Name => Statistic.Name;

        public bool ErrorIsPossible =>
            Statistic.Training.TrainingType == TrainingType.SequentialLearning
            || Statistic.Training.TrainingType == TrainingType.GoNoGo
            || Statistic.Training.TrainingTrials.Any(t => t.Parts.Any(p => p.Images.Any(i => i.Correctness == Correctness.False)));

        public TrainingsStatisticViewModel(TrainingStatistic statistic)
        {
            Statistic = statistic;

            Translator.Instance.PropertyChanged += TranslatorPropertyChanged;
        }

        private void TranslatorPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(this.Name));
        }
    }
}