using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using MriBase.App.Base.Views;
using MriBase.App.Dog.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(TouchscreenCalibrationPage), typeof(FullscreenPageRenderer))]
[assembly: ExportRenderer(typeof(BaseTrainingPage), typeof(FullscreenPageRenderer))]
namespace MriBase.App.Dog.Droid
{
    public class FullscreenPageRenderer : PageRenderer
    {
        private readonly Activity activity;
        private readonly int originalUiOptions;

        public FullscreenPageRenderer(Context context) : base(context)
        {
            this.activity = context.GetActivity();
            this.originalUiOptions = (int)activity.Window.DecorView.SystemUiVisibility;
        }

        protected override void OnAttachedToWindow()
        {
            var attrs = this.activity.Window.Attributes;
            attrs.Flags |= WindowManagerFlags.Fullscreen;
            this.activity.Window.Attributes = attrs;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
            {
                this.activity.Window.SetDecorFitsSystemWindows(false);

                if (this.activity.Window.InsetsController != null)
                {
                    this.activity.Window.InsetsController.Hide(WindowInsets.Type.NavigationBars());
                    this.activity.Window.InsetsController.Hide(WindowInsets.Type.StatusBars());
                    this.activity.Window.InsetsController.Hide(WindowInsets.Type.SystemBars());
                }
            }
            else
            {
                var uiOptions = (int)this.activity.Window.DecorView.SystemUiVisibility;
                var newUiOptions = (int)uiOptions;

                newUiOptions |=
                    (int)SystemUiFlags.LowProfile |
                    (int)SystemUiFlags.HideNavigation |
                    (int)SystemUiFlags.Fullscreen |
                    (int)SystemUiFlags.ImmersiveSticky;

                this.activity.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)newUiOptions;
            }
        }

        protected override void OnDetachedFromWindow()
        {
            activity.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)originalUiOptions;

            var attrs = this.activity.Window.Attributes;
            attrs.Flags &= ~WindowManagerFlags.Fullscreen;
            this.activity.Window.Attributes = attrs;
        }
    }
}