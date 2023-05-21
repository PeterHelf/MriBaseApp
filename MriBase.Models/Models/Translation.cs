using MriBase.Models.Interfaces;
using Newtonsoft.Json;
using System;

namespace MriBase.Models.Models
{
    [Serializable]
    public class Translation : ITranslation
    {
        /// <summary>
        /// Leerer Constructor für JSON Deserialisierung
        /// </summary>
        [JsonConstructor]
        private Translation()
        {
        }

        public Translation(string twoLetterIsoLanguageCode, string text)
        {
            TwoLetterIsoLanguageCode = twoLetterIsoLanguageCode;
            Text = text;
        }

        public string TwoLetterIsoLanguageCode { get; set; }

        public string Text { get; set; }
    }
}
