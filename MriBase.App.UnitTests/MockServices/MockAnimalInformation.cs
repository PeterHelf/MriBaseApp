using MriBase.Models.Enums;
using MriBase.Models.Interfaces;
using MriBase.Models.Models;
using System;
using System.Collections.Generic;

namespace MriBase.App.UnitTests.MockServices
{
    internal class MockAnimalInformation : IAnimalInformation
    {
        public MockAnimalInformation(DateTime dateOfBirth, Gender sex, int id, byte[] image, string name)
        {
            this.DateOfBirth = dateOfBirth;
            this.Sex = sex;
            this.Id = id;
            this.Image = image;
            this.Name = name;
            this.Statistics = new List<TrainingStatistic>();
        }

        public DateTime DateOfBirth { get; set; }
        public Gender Sex { get; set; }
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public string Name { get; set; }
        public List<TrainingStatistic> Statistics { get; set; }
    }
}
