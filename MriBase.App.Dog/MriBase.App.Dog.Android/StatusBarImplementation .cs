using Android.Views;
using MriBase.App.Base.Interfaces;
using MriBase.App.Dog.Droid;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(StatusBarImplementation))]
namespace MriBase.App.Dog.Droid
{
    public class StatusBarImplementation : IStatusBar
    {
        public StatusBarImplementation()
        {
        }

        WindowManagerFlags _originalFlags;

        #region IStatusBar implementation

        public void HideStatusBar()
        {
            var activity = Platform.CurrentActivity;
            var attrs = activity.Window.Attributes;
            _originalFlags = attrs.Flags;
            attrs.Flags |= Android.Views.WindowManagerFlags.Fullscreen;
            activity.Window.Attributes = attrs;
        }

        public void ShowStatusBar()
        {
            var activity = Platform.CurrentActivity;
            var attrs = activity.Window.Attributes;
            attrs.Flags = _originalFlags;
            activity.Window.Attributes = attrs;
        }

        #endregion
    }

}