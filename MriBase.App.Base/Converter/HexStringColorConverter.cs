using System;
using System.Globalization;
using Xamarin.Forms;
using Color = Xamarin.Forms.Color;

namespace MriBase.App.Base.Converter
{
    public class HexStringColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var hexString = value as string;
            return Color.FromHex(hexString);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Color)value).ToHex();
        }
    }
}
