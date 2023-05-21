using System.Collections.Generic;

namespace MriBase.Models.Interfaces
{
    public interface ITranslatableText
    {
        IEnumerable<ITranslation> Translations { get; }

        string this[string twoLetterISOcode]
        {
            get;
        }
    }
}
