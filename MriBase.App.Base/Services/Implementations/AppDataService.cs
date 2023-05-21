using MriBase.App.Base.Services.Interfaces;
using MriBase.Models.Interfaces;
using MriBase.Models.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MriBase.App.Base.Services.Implementations
{
    public class AppDataService : IAppDataService
    {
        private IAnimalInformation selectedAnimal;

        public UserData LogedInUser { get; set; }

        public ObservableCollection<IAnimalInformation> Animals { get; set; }

        public IAnimalInformation SelectedAnimal
        {
            get => selectedAnimal;
            set
            {
                selectedAnimal = value;
                SelectedAnimalChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        public Task<Training[]> Trainings { get; set; }
        public UserSettings UserSettings { get; set; }

        public bool IsLogedInOnline { get; set; }

        public event EventHandler SelectedAnimalChanged;
    }
}
