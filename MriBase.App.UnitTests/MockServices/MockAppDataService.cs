using MriBase.App.Base.Services.Interfaces;
using MriBase.Models.Interfaces;
using MriBase.Models.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MriBase.App.UnitTests.MockServices
{
    internal class MockAppDataService : IAppDataService
    {
        public ObservableCollection<IAnimalInformation> Animals { get; set; }
        public UserData LogedInUser { get; set; }
        public IAnimalInformation SelectedAnimal { get; set; }
        public Task<Training[]> Trainings { get; set; }
        public UserSettings UserSettings { get; set; }
        public bool IsLogedInOnline { get; set; }

        public event EventHandler SelectedAnimalChanged { add { } remove { } }
    }
}