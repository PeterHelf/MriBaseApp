using MriBase.App.Base.Converter;
using MriBase.Models.Enums;

namespace MriBase.App.Base.ViewModels
{
    public class AnimalBreedViewModel
    {
        private static readonly EnumLanguageConverter Converter = new EnumLanguageConverter();

        public AnimalBreedViewModel(Breed breed)
        {
            this.Breed = breed;
            this.BreedName = Converter.Convert(Breed, typeof(string)) as string;
        }

        public Breed Breed { get; }

        public string BreedName { get; }

        public override string ToString()
        {
            return this.BreedName;
        }
    }
}