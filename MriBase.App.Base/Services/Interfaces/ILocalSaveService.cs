using MriBase.Models.Interfaces;
using MriBase.Models.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MriBase.App.Base.Services.Interfaces
{
    public interface ILocalSaveService
    {
        List<IAnimalInformation> RememberedAnimals { get; set; }
        List<UserData> RememberedUsers { get; set; }

        Task LoadAnimals();
        List<int> LoadChangedAnimalIds();
        List<TrainingSessionResult> LoadOfflineResults();
        List<Training> LoadTrainings();
        Task LoadUsers();
        UserSettings LoadUserSettings();
        SavedSessionProgress LoadSessionProgress(int trainingId);
        void SaveChangedAnimnalIds(List<int> changedAnimalIds);
        Task SaveAnimals();
        void SaveOfflineResults(List<TrainingSessionResult> results);
        void SaveTrainings(List<Training> trainings);
        Task SaveUsers();
        void SaveUserSettings();
        void SaveSessionProgress(List<SavedSessionProgress> progress);
        void SaveSessionProgress(SavedSessionProgress newProgress);

        Task LoadUsersTask { get; }
        Task LoadAnimalsTask { get; }
    }
}