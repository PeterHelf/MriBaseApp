using System.Collections.Generic;

namespace MriBase.Models.Models
{
    public class ProjectAnimals
    {
        public ProjectAnimals(string animalTypeName, List<AnimalInformation> animals)
        {
            AnimalTypeName = animalTypeName;
            Animals = animals;
        }

        public string AnimalTypeName { get; set; }

        public List<AnimalInformation> Animals { get; set; }
    }
}
