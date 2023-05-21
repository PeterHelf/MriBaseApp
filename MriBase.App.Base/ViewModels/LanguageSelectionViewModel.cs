using MriBase.App.Base.Services.Interfaces;
using MriBase.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace MriBase.App.Base.ViewModels
{
    public class LanguageSelectionViewModel : BaseViewModel
    {
        private readonly IAppDataService appDataService;
        private readonly ILocalSaveService localSaveService;

        public IEnumerable<AvailableLanguages> Languages => Enum.GetValues(typeof(AvailableLanguages)).Cast<AvailableLanguages>();

        public Command LanguageSelectionCommand { get; set; }

        public LanguageSelectionViewModel(IAppDataService appDataService, ILocalSaveService localSaveService)
        {
            this.appDataService = appDataService;
            this.localSaveService = localSaveService;
            this.LanguageSelectionCommand = new Command(l =>
                {
                    this.appDataService.UserSettings.Language = (AvailableLanguages)l;
                    this.localSaveService.SaveUserSettings();

                    OnPropertyChanged(null);
                });
        }
    }
}