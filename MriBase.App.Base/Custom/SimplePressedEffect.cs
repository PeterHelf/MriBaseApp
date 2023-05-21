using System.Windows.Input;
using Xamarin.Forms;

namespace MriBase.App.Base.Custom
{
    public class SimplePressedEffect : RoutingEffect
    {
        public SimplePressedEffect() : base("MriBase.App.SimplePressedEffect")
        {
        }

        public static readonly BindableProperty PressedCommandProperty = BindableProperty.CreateAttached("PressedCommand", typeof(ICommand), typeof(SimplePressedEffect), (object)null);
        public static ICommand GetPressedCommand(BindableObject view)
        {
            return (ICommand)view.GetValue(PressedCommandProperty);
        }

        public static void SetPressedCommand(BindableObject view, ICommand value)
        {
            view.SetValue(PressedCommandProperty, value);
        }


        public static readonly BindableProperty ParameterProperty = BindableProperty.CreateAttached("Parameter", typeof(object), typeof(SimplePressedEffect), (object)null);
        public static object GetParameter(BindableObject view)
        {
            return view.GetValue(ParameterProperty);
        }

        public static void SetParameter(BindableObject view, object value)
        {
            view.SetValue(ParameterProperty, value);
        }
    }
}
