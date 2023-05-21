using System.Threading.Tasks;
using Xamarin.Forms;

namespace MriBase.App.Base.Services.Interfaces
{
    public interface INavigationService
    {
        Task NavigateToAsync<T>() where T : Page;
        Task NavigateToAsync(Page page);
        Task NavigateToWithFactoryAsync<T, U>(U item) where T : Page;
        void ClearNavigationStack();
        Task<Page> ReturnToLastPage(bool animated = true);
        Task ReturnToLoginPage();
    }
}
