using Akavache;
using MriBase.App.Base.Services.Interfaces;
using MriBase.Models;
using MriBase.Models.Interfaces;
using MriBase.Models.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MriBase.App.Dog.Services.Implementations
{
    public class DogLocalSaveService : ILocalSaveService
    {
        private readonly IAppDataService appDataService;

        public List<UserData> RememberedUsers { get; set; }
        public List<IAnimalInformation> RememberedAnimals { get; set; }

        public Task LoadUsersTask { get; }
        public Task LoadAnimalsTask { get; }

        public DogLocalSaveService(IAppDataService appDataService, IConfigService configService)
        {
            Registrations.Start(configService.AppName);
            RememberedUsers = new List<UserData>();
            RememberedAnimals = new List<IAnimalInformation>();
            this.LoadUsersTask = LoadUsers();
            this.LoadAnimalsTask = LoadAnimals();

            this.appDataService = appDataService ?? throw new ArgumentNullException(nameof(appDataService));
        }

        public List<TrainingSessionResult> LoadOfflineResults()
        {
            return LoadData<List<TrainingSessionResult>>("Results", "OfflineResults.bin");
        }

        public List<int> LoadChangedAnimalIds()
        {
            return LoadData<List<int>>("AnimalChanges", "AnimalIds.bin");
        }

        public UserSettings LoadUserSettings()
        {
            return LoadData<UserSettings>("SavedUsers", "UserSettings.bin");
        }

        public List<Training> LoadTrainings()
        {
            var trainingList = new List<Training>();

            var configFileName = GetType().GetTypeInfo().Assembly.GetName().Name + ".appsettings.json";

            foreach (var resourceName in GetType().GetTypeInfo().Assembly.GetManifestResourceNames())
            {
                if (resourceName == configFileName)
                {
                    continue;
                }

                var trainingJsonStream = GetType().GetTypeInfo().Assembly.GetManifestResourceStream(resourceName);

                var serializer = new Newtonsoft.Json.JsonSerializer();

                using (var sr = new StreamReader(trainingJsonStream))
                using (var jsonTextReader = new JsonTextReader(sr))
                {
                    var training = serializer.Deserialize<Training>(jsonTextReader);
                    trainingList.Add(training);
                }
            }

            return trainingList;            
            //return LoadData<List<Training>>("SavedTrainings", "Trainings.bin");
        }

        public async Task LoadUsers()
        {
            try
            {
                this.RememberedUsers = await BlobCache.Secure.GetObject<List<UserData>>("users");
            }
            catch (Exception)
            {
                this.RememberedUsers = new List<UserData>();
            }
        }

        public async Task LoadAnimals()
        {
            try
            {
                this.RememberedAnimals = (await BlobCache.Secure.GetObject<List<DogInformation>>("animals")).Select(a => a as IAnimalInformation).ToList();
            }
            catch (Exception)
            {
                this.RememberedAnimals = new List<IAnimalInformation>();
            }
        }

        public SavedSessionProgress LoadSessionProgress(int trainingId)
        {
            return LoadData<List<SavedSessionProgress>>("SessionProgress", "SessionProgress.bin").FirstOrDefault(s => s.TrainingId == trainingId);
        }

        public async Task SaveUsers()
        {
            var index = RememberedUsers.FindIndex(u => u.UserId == appDataService.LogedInUser.UserId);

            if (index == -1)
            {
                RememberedUsers.Add(appDataService.LogedInUser);
            }
            else
            {
                RememberedUsers[RememberedUsers.FindIndex(u => u.UserId == appDataService.LogedInUser.UserId)] = appDataService.LogedInUser;
            }

            await BlobCache.Secure.InsertObject("users", RememberedUsers);
        }

        public async Task SaveAnimals()
        {
            foreach (var animal in appDataService.Animals)
            {
                var index = RememberedAnimals.FindIndex(a => a.Id == animal.Id);

                if (index == -1)
                {
                    RememberedAnimals.Add(animal);
                }
                else
                {
                    RememberedAnimals[RememberedAnimals.FindIndex(a => a.Id == animal.Id)] = animal;
                }

            }

            await BlobCache.Secure.InsertObject("animals", RememberedAnimals);
        }

        public void SaveChangedAnimnalIds(List<int> changedAnimalIds)
        {
            SaveData("AnimalChanges", "AnimalIds.bin", changedAnimalIds);
        }

        public void SaveUserSettings()
        {
            SaveData("SavedUsers", "UserSettings.bin", this.appDataService.UserSettings);
        }

        public void SaveTrainings(List<Training> trainings)
        {
            SaveData("SavedTrainings", "Trainings.bin", trainings);
        }

        public void SaveOfflineResults(List<TrainingSessionResult> results)
        {
            SaveData("Results", "OfflineResults.bin", results);
        }

        public void SaveSessionProgress(List<SavedSessionProgress> progress)
        {
            SaveData("SessionProgress", "SessionProgress.bin", progress);
        }

        public void SaveSessionProgress(SavedSessionProgress newProgress)
        {
            var sessionProgress = LoadData<List<SavedSessionProgress>>("SessionProgress", "SessionProgress.bin");

            var index = sessionProgress.FindIndex(s => s.TrainingId == newProgress.TrainingId && s.AnimalId == this.appDataService.SelectedAnimal.Id);

            if (index == -1)
            {
                sessionProgress.Add(newProgress);
            }
            else
            {
                sessionProgress[index] = newProgress;
            }

            SaveData("SessionProgress", "SessionProgress.bin", sessionProgress);
        }

        private T LoadData<T>(string folder, string filename)
                        where T : new()
        {
            var filePath = Path.Combine(FileSystem.AppDataDirectory, folder, filename);
            if (File.Exists(filePath))
            {
                using (var stream = File.OpenRead(filePath))
                {
                    IFormatter formatter = new BinaryFormatter();

                    try
                    {
                        return (T)formatter.Deserialize(stream);
                    }
                    catch (Exception)
                    {
                        return new T();
                    }
                }
            }

            return new T();
        }

        private void SaveData(string folder, string filename, object data)
        {
            var filePath = Path.Combine(FileSystem.AppDataDirectory, folder, filename);
            Directory.CreateDirectory(Path.Combine(FileSystem.AppDataDirectory, folder));
            using (var stream = File.Open(filePath, FileMode.Create))
            {
                IFormatter formatter = new BinaryFormatter();

                formatter.Serialize(stream, data);
            }
        }
    }
}