using MriBase.Models.Interfaces;
using MriBase.Models.Resources;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace MriBase.Models.Translation
{
    public class Translator : INotifyPropertyChanged
    {
        public string this[string text]
        {
            get
            {
                var splitText = text.Split('_');
                var resourceManager = new ResourceManager("MriBase.Models.Resources." + splitText[0],
                    Assembly.GetAssembly(typeof(ResViewLogin)));

                return resourceManager.GetString(splitText[1].Trim(), CultureInfo.CurrentCulture);
            }
        }

        public static Translator Instance { get; } = new Translator();

        public event PropertyChangedEventHandler PropertyChanged;

        public string TranslateText(string resourceName, CultureInfo culture)
        {
            var splitText = resourceName.Split('.');
            var resourceManager = new ResourceManager("MriBase.Models.Resources." + splitText[0],
                Assembly.GetAssembly(typeof(ResViewLogin)));

            return resourceManager.GetString(splitText[1].Trim(), culture);
        }

        public string TranslateText(ITranslatableText text)
        {
            return this.TranslateText(text, CultureInfo.CurrentCulture);
        }


        public string TranslateText(ITranslatableText text, CultureInfo culture)
        {
            if (text is null)
            {
                return null;
            }

            var currentTwoLetterISO = culture.TwoLetterISOLanguageName;

            return text[currentTwoLetterISO];
        }

        public string this[ITranslatableText text]
        {
            get
            {
                return TranslateText(text);
            }
        }

        public void Invalidate()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
    }
}