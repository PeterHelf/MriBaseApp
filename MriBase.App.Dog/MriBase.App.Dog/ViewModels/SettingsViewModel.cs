using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.ViewModels;
using MriBase.Models.Services.Interfaces;

namespace MriBase.App.Dog.ViewModels
{
    public class SettingsViewModel : SettingsViewModelBase
    {
        public SettingsViewModel(INavigationService navigationService, IImageRecourceService imageRecourceService) : base(navigationService, imageRecourceService)
        {
        }

        public override bool LogoutPossible => false;
    }
}