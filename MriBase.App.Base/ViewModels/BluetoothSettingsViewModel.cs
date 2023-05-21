using MriBase.App.Base.Services.Interfaces;
using MriBase.Models.Models;
using MriBase.Models.Resources;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace MriBase.App.Base.ViewModels
{
    public class BluetoothSettingsViewModel : BaseViewModel
    {
        public Command AddFeederName { get; set; }
        public Command AddServiceGUID { get; set; }
        public Command AddReadCharacteristicGUID { get; set; }
        public Command AddWriteCharacteristicGUID { get; set; }
        public Command DeleteFeederName { get; }
        public Command DeleteServiceGUID { get; }
        public Command DeleteReadCharacteristicGUID { get; }
        public Command DeleteWriteCharacteristicGUID { get; }

        private readonly IAppDataService appDataService;
        private readonly ILocalSaveService localSaveService;

        public BluetoothSettings BluetoothSettings { get; }

        public ObservableCollection<string> FeederNames { get; }
        public ObservableCollection<Guid> FeederReadCharacteristicIds { get; }
        public ObservableCollection<Guid> FeederServiceUIDs { get; }
        public ObservableCollection<Guid> FeederWriteCharacteristicIds { get; }

        public BluetoothSettingsViewModel(IAppDataService appDataService, ILocalSaveService localSaveService)
        {
            this.appDataService = appDataService;
            this.localSaveService = localSaveService;
            this.BluetoothSettings = this.appDataService.UserSettings.BluetoothSettings;
            this.FeederNames = new ObservableCollection<string>(this.appDataService.UserSettings.BluetoothSettings.FeederNames);
            this.FeederReadCharacteristicIds = new ObservableCollection<Guid>(this.appDataService.UserSettings.BluetoothSettings.FeederReadCharacteristicIds);
            this.FeederServiceUIDs = new ObservableCollection<Guid>(this.appDataService.UserSettings.BluetoothSettings.FeederServiceUIDs);
            this.FeederWriteCharacteristicIds = new ObservableCollection<Guid>(this.appDataService.UserSettings.BluetoothSettings.FeederWriteCharacteristicIds);

            this.AddFeederName = new Command(async () =>
            {
                var result = string.Empty;
                await Device.InvokeOnMainThreadAsync(async () => result = await Application.Current.MainPage.DisplayPromptAsync(ResViewBluetoothSettings.FeederName, ResViewBluetoothSettings.WhatIsTheFeederName));

                if (result is null)
                {
                    return;
                }

                if (!string.IsNullOrWhiteSpace(result) && !BluetoothSettings.FeederNames.Contains(result.Trim()))
                {
                    BluetoothSettings.FeederNames.Add(result.Trim());
                    this.FeederNames.Add(result.Trim());
                    this.localSaveService.SaveUserSettings();
                }
            });


            this.AddServiceGUID = new Command(async () =>
            {
                var result = string.Empty;
                await Device.InvokeOnMainThreadAsync(async () => result = await Application.Current.MainPage.DisplayPromptAsync(ResViewBluetoothSettings.FeederServiceId, ResViewBluetoothSettings.WhatIsTheServiceId));

                if (result is null)
                {
                    return;
                }

                if (Guid.TryParse(result, out Guid guid))
                {
                    if (!BluetoothSettings.FeederServiceUIDs.Contains(guid))
                    {
                        BluetoothSettings.FeederServiceUIDs.Add(guid);
                        this.FeederServiceUIDs.Add(guid);
                        this.localSaveService.SaveUserSettings();
                    }
                }
                else
                {
                    await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayAlert(ResViewBluetoothSettings.InvalidId, ResViewBluetoothSettings.IdIsInvalid, ResViewBasics.Ok));
                }
            });

            this.AddReadCharacteristicGUID = new Command(async () =>
            {
                var result = string.Empty;
                await Device.InvokeOnMainThreadAsync(async () => result = await Application.Current.MainPage.DisplayPromptAsync(ResViewBluetoothSettings.CharacteristicId, ResViewBluetoothSettings.WhatIsTheCharacteristicId));

                if (result is null)
                {
                    return;
                }

                if (Guid.TryParse(result, out Guid guid))
                {
                    if (!BluetoothSettings.FeederReadCharacteristicIds.Contains(guid))
                    {
                        BluetoothSettings.FeederReadCharacteristicIds.Add(guid);
                        this.FeederReadCharacteristicIds.Add(guid);
                        this.localSaveService.SaveUserSettings();
                    }
                }
                else
                {
                    await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayAlert(ResViewBluetoothSettings.InvalidId, ResViewBluetoothSettings.IdIsInvalid, ResViewBasics.Ok));
                }
            });

            this.AddWriteCharacteristicGUID = new Command(async () =>
            {
                var result = string.Empty;
                await Device.InvokeOnMainThreadAsync(async () => result = await Application.Current.MainPage.DisplayPromptAsync(ResViewBluetoothSettings.CharacteristicId, ResViewBluetoothSettings.WhatIsTheCharacteristicId));

                if (result is null)
                {
                    return;
                }

                if (Guid.TryParse(result, out Guid guid))
                {
                    if (!BluetoothSettings.FeederWriteCharacteristicIds.Contains(guid))
                    {
                        BluetoothSettings.FeederWriteCharacteristicIds.Add(guid);
                        this.FeederWriteCharacteristicIds.Add(guid);
                        this.localSaveService.SaveUserSettings();
                    }
                }
                else
                {
                    await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayAlert(ResViewBluetoothSettings.InvalidId, ResViewBluetoothSettings.IdIsInvalid, ResViewBasics.Ok));
                }
            });

            this.DeleteFeederName = new Command(obj =>
            {
                var name = obj as string;

                this.FeederNames.Remove(name);
                this.BluetoothSettings.FeederNames.Remove(name);
                this.localSaveService.SaveUserSettings();
            });

            this.DeleteServiceGUID = new Command(obj =>
            {
                var guid = (Guid)obj;

                this.FeederServiceUIDs.Remove(guid);
                this.BluetoothSettings.FeederServiceUIDs.Remove(guid);
                this.localSaveService.SaveUserSettings();
            });

            this.DeleteReadCharacteristicGUID = new Command(obj =>
            {
                var guid = (Guid)obj;

                this.FeederReadCharacteristicIds.Remove(guid);
                this.BluetoothSettings.FeederReadCharacteristicIds.Remove(guid);
                this.localSaveService.SaveUserSettings();
            });

            this.DeleteWriteCharacteristicGUID = new Command(obj =>
            {
                var guid = (Guid)obj;

                this.FeederWriteCharacteristicIds.Remove(guid);
                this.BluetoothSettings.FeederWriteCharacteristicIds.Remove(guid);
                this.localSaveService.SaveUserSettings();
            });
        }
    }
}
