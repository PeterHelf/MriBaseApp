namespace MriBase.Models.Interfaces
{
    public interface ITranslation
    {
        string TwoLetterIsoLanguageCode { get; }

        string Text { get; }
    }
}
