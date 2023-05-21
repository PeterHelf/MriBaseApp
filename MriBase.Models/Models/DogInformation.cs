using MriBase.Models.Enums;
using System;
using System.Collections.Generic;

namespace MriBase.Models.Models
{
    [Serializable]
    public class DogInformation : AnimalInformation
    {
        public Breed Breed { get; set; }

        public int OwnerId { get; set; }

        public DogInformation(string name, int ownerId, byte[] image, DateTime dateOfBirth, Gender sex, Breed breed)
        {
            Name = name;
            DateOfBirth = dateOfBirth;
            Sex = sex;
            this.OwnerId = ownerId;
            Breed = breed;
            this.Image = image;

            this.Statistics = new List<TrainingStatistic>();
        }
    }
}