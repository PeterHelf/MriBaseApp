using MriBase.Models.Enums;
using MriBase.Models.Models;
using System;
using System.Collections.Generic;

namespace MriBase.Models.Interfaces
{
    public interface IAnimalInformation
    {
        DateTime DateOfBirth { get; set; }
        Gender Sex { get; set; }
        int Id { get; set; }
        byte[] Image { get; set; }
        string Name { get; set; }
        List<TrainingStatistic> Statistics { get; set; }
    }
}
