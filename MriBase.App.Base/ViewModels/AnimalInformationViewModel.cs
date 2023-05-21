using MriBase.Models.Interfaces;
using System.IO;
using Xamarin.Forms;

namespace MriBase.App.Base.ViewModels
{
    public class AnimalInformationViewModel : BaseViewModel
    {
        public IAnimalInformation AnimalInformation { get; }

        public string Name => this.AnimalInformation.Name;

        public ImageSource Image => ImageSource.FromStream(() => new MemoryStream(this.AnimalInformation.Image));

        public AnimalInformationViewModel(IAnimalInformation animalInformation)
        {
            AnimalInformation = animalInformation;
        }
    }
}