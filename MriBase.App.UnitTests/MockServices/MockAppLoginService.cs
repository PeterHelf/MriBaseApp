using MriBase.App.Base.Services.Interfaces;
using MriBase.Models.Enums;
using MriBase.Models.Interfaces;
using MriBase.Models.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MriBase.App.UnitTests.MockServices
{
    internal class MockAppLoginService : ILoginService
    {
        private readonly IAppDataService appDataService;

        public MockAppLoginService(IAppDataService appDataService)
        {
            this.appDataService = appDataService ?? throw new System.ArgumentNullException(nameof(appDataService));
        }

        public Task<LoginError> Login(string userName, string passwordHash)
        {
            this.appDataService.LogedInUser = new UserData("TestUser", string.Empty);
            this.appDataService.Animals = new ObservableCollection<IAnimalInformation>() { new MockAnimalInformation(new System.DateTime(2007, 3, 25), Gender.Female, 1, null, "Animal") };
            return Task.FromResult(LoginError.NoError);
        }

        public Task<LoginError> LoginOffline(string userName, string passwordHash, string messageTitle, string messageText)
        {
            throw new System.NotImplementedException();
        }
    }
}