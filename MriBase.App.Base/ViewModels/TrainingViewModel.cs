using MriBase.Models.Models;
using MriBase.Models.Translation;
using System.IO;
using Xamarin.Forms;

namespace MriBase.App.Base.ViewModels
{
    public class TrainingViewModel : BaseViewModel
    {
        public TrainingViewModel(Training training)
        {
            Training = training;
            Translator.Instance.PropertyChanged += TranslatorPropertyChanged;
        }

        private void TranslatorPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(this.Name));
        }

        public Training Training { get; }
        public string Name => Translator.Instance.TranslateText(this.Training.Name);
        public string Description => Training.Description;
        public ImageSource Image => ImageSource.FromStream(() => new MemoryStream(Training.Image));
    }
}