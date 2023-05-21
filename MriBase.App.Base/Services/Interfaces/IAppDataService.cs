using MriBase.Models.Interfaces;
using MriBase.Models.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MriBase.App.Base.Services.Interfaces
{
    public interface IAppDataService
    {
        ObservableCollection<IAnimalInformation> Animals { get; set; }
        UserData LogedInUser { get; set; }
        IAnimalInformation SelectedAnimal { get; set; }
        event EventHandler SelectedAnimalChanged;
        Task<Training[]> Trainings { get; set; }
        UserSettings UserSettings { get; set; }
        bool IsLogedInOnline { get; set; }
    }
}