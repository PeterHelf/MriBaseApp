using MriBase.Models.Enums;
using MriBase.Models.Translation;
using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace MriBase.Models.Models
{
    [Serializable]
    public class UserSettings
    {
        private AvailableLanguages language;
        public float Volume { get; set; }

        public bool StayLogedIn { get; set; }

        public AvailableLanguages Language
        {
            get => language;
            set
            {
                language = value;

                this.UpdateAppLanguage();
            }
        }

        public BluetoothSettings BluetoothSettings { get; set; }

        public void UpdateAppLanguage()
        {
            CultureInfo newCulture;

            switch (this.language)
            {
                case AvailableLanguages.German:
                    newCulture = new CultureInfo("de");
                    break;
                case AvailableLanguages.English:
                    newCulture = new CultureInfo("en");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(this.language), this.language, null);
            }

            CultureInfo.CurrentCulture = newCulture;
            CultureInfo.CurrentUICulture = newCulture;
            Translator.Instance.Invalidate();
        }

        public UserSettings()
        {
            this.Volume = 0.5f;
            this.StayLogedIn = false;
            this.BluetoothSettings = new BluetoothSettings();

            switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
            {
                case "de":
                    this.Language = AvailableLanguages.German;
                    break;
                default:
                    this.Language = AvailableLanguages.English;
                    break;
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (this.BluetoothSettings is null)
            {
                this.BluetoothSettings = new BluetoothSettings();
            }
        }
    }
}