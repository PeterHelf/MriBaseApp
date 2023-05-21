using MriBase.Models.Translation;
using System;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace MriBase.App.Base.Converter
{
    public class EnumLanguageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter = null, CultureInfo culture = null)
        {
            if (!(value is Enum enumObj))
            {
                return string.Empty;
            }

            var enumType = enumObj.GetType().ToString().Split('.').Last();
            var enumValue = enumObj.ToString().Replace('.', '_');

            return Translator.Instance["ResView" + enumType + '_' + enumValue];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}