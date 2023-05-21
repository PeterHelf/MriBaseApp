using MriBase.Models.Enums;
using MriBase.Models.Models;
using System.IO;
using Xamarin.Forms;
using Point = System.Drawing.Point;

namespace MriBase.App.Base.ViewModels
{
    public class TrainingImageViewModel
    {
        public TrainingImageViewModel(TrainingImage trainingImage)
        {
            TrainingsImage = trainingImage;
        }

        public TrainingImage TrainingsImage { get; }

        public ImageSource Image => ImageSource.FromStream(() => new MemoryStream(TrainingsImage.Image));
        public Correctness Correctness => TrainingsImage.Correctness;
        public double ImageSize => TrainingsImage.ImageSize;
        public double ImageTouchableBorderSize => TrainingsImage.ImageTouchableBorderSize;
        public Point Position { get; set; }
    }
}