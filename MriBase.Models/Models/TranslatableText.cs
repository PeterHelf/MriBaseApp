using MriBase.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MriBase.Models.Models
{
    [Serializable]
    public class TranslatableText : ITranslatableText
    {
        public TranslatableText()
        {
            this.Translations = new List<Translation>();
        }

        public string this[string twoLetterISOcode]
        {
            get
            {
                var text = this.Translations.FirstOrDefault(t => t.TwoLetterIsoLanguageCode == twoLetterISOcode)?.Text
                    ?? this.Translations.FirstOrDefault(t => t.TwoLetterIsoLanguageCode == "de")?.Text;

                if (text is null)
                {
                    return this.Translations.FirstOrDefault()?.Text;
                }
                else
                {
                    return text;
                }
            }

            set
            {
                if (!(this.Translations.FirstOrDefault(t => t.TwoLetterIsoLanguageCode == twoLetterISOcode) is null))
                {
                    this.Translations.FirstOrDefault(t => t.TwoLetterIsoLanguageCode == twoLetterISOcode).Text = value;
                }
                else if (!(this.Translations.FirstOrDefault(t => t.TwoLetterIsoLanguageCode == "de") is null))
                {
                    this.Translations.FirstOrDefault(t => t.TwoLetterIsoLanguageCode == "de").Text = value;
                }
                else if (!(this.Translations.FirstOrDefault() is null))
                {
                    this.Translations.FirstOrDefault().Text = value;
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
        }

        public List<Translation> Translations { get; set; }

        IEnumerable<ITranslation> ITranslatableText.Translations => Translations;

        public override string ToString()
        {
            return this[CultureInfo.CurrentCulture.TwoLetterISOLanguageName];
        }
    }
}
