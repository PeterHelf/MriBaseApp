using MriBase.Models.Enums;
using System;
using System.Collections.Generic;

namespace MriBase.Models.Models
{
    [Serializable]
    public class BirdInformation : AnimalInformation
    {
        public BirdInformation(string name, byte[] image, DateTime dateOfBirth, Gender sex)
        {
            Name = name;
            DateOfBirth = dateOfBirth;
            Sex = sex;
            this.Image = image;

            this.Statistics = new List<TrainingStatistic>();
        }

        public BirdInformation()
        {
            this.Statistics = new List<TrainingStatistic>();
        }
    }
}
