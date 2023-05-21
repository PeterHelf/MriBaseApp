using MriBase.Models.Enums;
using MriBase.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace MriBase.Models.Models
{
    [Serializable]
    public class AnimalInformation : IAnimalInformation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Sex { get; set; }
        public byte[] Image { get; set; }
        public List<TrainingStatistic> Statistics { get; set; }
    }
}
