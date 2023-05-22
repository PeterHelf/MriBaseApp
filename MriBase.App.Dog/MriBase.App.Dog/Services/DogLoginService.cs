using Acr.Collections;
using MriBase.App.Base.Services.Interfaces;
using MriBase.Models.Enums;
using MriBase.Models.Interfaces;
using MriBase.Models.Models;
using MriBase.Models.Resources;
using MriBase.Models.Services.Interfaces;
using Plugin.Connectivity;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MriBase.App.Dog.Services.Implementations
{
    public class DogLoginService : ILoginService
    {
        private readonly IOfflineChangesManager offlineChangesManager;
        private readonly ILocalSaveService localSaveService;
        private readonly IAppDataService appDataService;

        public DogLoginService(IOfflineChangesManager offlineChangesManager, ILocalSaveService localSaveService, IAppDataService appDataService)
        {
            this.offlineChangesManager = offlineChangesManager ?? throw new ArgumentNullException(nameof(offlineChangesManager));
            this.localSaveService = localSaveService ?? throw new ArgumentNullException(nameof(localSaveService));
            this.appDataService = appDataService ?? throw new ArgumentNullException(nameof(appDataService));
        }

        public async Task<LoginError> LoginOffline(string userName, string passwordHash)
        {
            await this.localSaveService.LoadUsersTask;
            this.appDataService.LogedInUser = this.localSaveService.RememberedUsers.FirstOrDefault(u => u.UserName == userName && u.Password == passwordHash);

            if (this.appDataService.LogedInUser is null)
            {
                var userData = new UserData(userName, passwordHash);
                userData.UserId = new Random().Next(int.MaxValue);
                this.localSaveService.RememberedUsers.Add(userData);
                this.appDataService.LogedInUser = userData;
                await this.localSaveService.SaveUsers();
            }

            await this.localSaveService.LoadAnimalsTask;
            this.appDataService.Animals = new ObservableCollection<IAnimalInformation>(localSaveService.RememberedAnimals.Where(a => a is DogInformation d && d.OwnerId == this.appDataService.LogedInUser.UserId));

            this.appDataService.IsLogedInOnline = false;
            return LoginError.NoError;
        }
    }
}
