using MriBase.App.Base.Services.Interfaces;
using MriBase.Models.Interfaces;
using MriBase.Models.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MriBase.App.UnitTests.MockServices
{
    internal class MockAppLocalSaveService : ILocalSaveService
    {
        public List<IAnimalInformation> RememberedAnimals { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public List<UserData> RememberedUsers { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void LoadAnimals()
        {
            throw new System.NotImplementedException();
        }

        public List<int> LoadChangedAnimalIds()
        {
            throw new System.NotImplementedException();
        }

        public List<TrainingSessionResult> LoadOfflineResults()
        {
            throw new System.NotImplementedException();
        }

        public SavedSessionProgress LoadSessionProgress(int trainingId)
        {
            throw new System.NotImplementedException();
        }

        public List<Training> LoadTrainings()
        {
            throw new System.NotImplementedException();
        }

        public void LoadUsers()
        {
            throw new System.NotImplementedException();
        }

        public UserSettings LoadUserSettings()
        {
            throw new System.NotImplementedException();
        }

        public Task SaveAnimals()
        {
            return Task.CompletedTask;
        }

        public void SaveChangedAnimnalIds(List<int> changedAnimalIds)
        {
            throw new System.NotImplementedException();
        }

        public void SaveOfflineResults(List<TrainingSessionResult> results)
        {
            throw new System.NotImplementedException();
        }

        public void SaveSessionProgress(List<SavedSessionProgress> progress)
        {
            throw new System.NotImplementedException();
        }

        public void SaveSessionProgress(SavedSessionProgress newProgress)
        {
            throw new System.NotImplementedException();
        }

        public void SaveTrainings(List<Training> trainings)
        {
            throw new System.NotImplementedException();
        }

        public Task SaveUsers()
        {
            throw new System.NotImplementedException();
        }

        public void SaveUserSettings()
        {
            throw new System.NotImplementedException();
        }
    }
}