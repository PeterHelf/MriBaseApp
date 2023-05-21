using Android.Views;
using MriBase.App.Base.Custom;
using MriBase.App.Dog.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("MriBase.App")]
[assembly: ExportEffect(typeof(AndroidSimplePressedEffect), "SimplePressedEffect")]
namespace MriBase.App.Dog.Droid
{
    public class AndroidSimplePressedEffect : PlatformEffect
    {
        private bool _attached;
        public AndroidSimplePressedEffect()
        {
        }

        protected override void OnAttached()
        {
            if (!_attached)
            {
                if (Control != null)
                {
                    Control.LongClickable = true;
                    Control.Touch += Control_Touch;
                }
                else
                {
                    Container.LongClickable = true;
                    Container.Touch += Control_Touch;
                }
                _attached = true;
            }
        }

        private void Control_Touch(object sender, Android.Views.View.TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Down)
            {
                var command = SimplePressedEffect.GetPressedCommand(Element);
                command?.Execute(SimplePressedEffect.GetParameter(Element));
            }
        }


        protected override void OnDetached()
        {
            if (_attached)
            {
                if (Control != null)
                {
                    Control.LongClickable = true;
                    Control.Touch -= Control_Touch;
                }
                else
                {
                    Container.LongClickable = true;
                    Container.Touch -= Control_Touch;
                }
                _attached = false;
            }
        }
    }
}