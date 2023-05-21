using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.Views;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MriBase.App.Base.Services.Implementations
{
    public class NavigationService : INavigationService
    {
        private readonly IContainer container;
        private readonly IAppDataService appDataService;

        public NavigationService(IContainer container, IAppDataService appDataService)
        {
            this.container = container ?? throw new ArgumentNullException(nameof(container));
            this.appDataService = appDataService ?? throw new ArgumentNullException(nameof(appDataService));
        }

        public Task NavigateToAsync<T>() where T : Page
        {
            var page = container.Resolve<T>();

            return InternalNavigateToAsync(page);
        }

        public Task NavigateToWithFactoryAsync<T, U>(U item) where T : Page
        {
            var factory = container.Resolve<IFactory<T, U>>();

            var page = factory.CreateInstance(item);

            return InternalNavigateToAsync(page);
        }

        public Task NavigateToAsync(Page page)
        {
            return InternalNavigateToAsync(page);
        }

        public void ClearNavigationStack()
        {
            var navigation = (Application.Current.MainPage as NavigationPage)?.Navigation;
            var navStack = navigation.NavigationStack.ToList();

            for (int i = 0; i < navStack.Count - 1; i++)
            {
                navigation.RemovePage(navStack[i]);
            }
        }

        private async Task InternalNavigateToAsync(Page page)
        {
            if (page is LoginPage)
            {
                Application.Current.MainPage = new NavigationPage(page);
            }
            else
            {
                if (Application.Current.MainPage is NavigationPage navigationPage)
                {
                    await navigationPage.PushAsync(page);
                }
                else
                {
                    Application.Current.MainPage = new NavigationPage(page);
                }
            }
        }

        public Task<Page> ReturnToLastPage(bool animated = true)
        {
            var navigation = (Application.Current.MainPage as NavigationPage)?.Navigation;

            return navigation.PopAsync(animated);
        }

        public async Task ReturnToLoginPage()
        {
            this.appDataService.LogedInUser = null;
            this.appDataService.IsLogedInOnline = false;

            await this.NavigateToAsync<LoginPage>();

            this.ClearNavigationStack();
        }
    }
}
