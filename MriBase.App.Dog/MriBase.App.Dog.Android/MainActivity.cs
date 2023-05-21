using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using MriBase.App.Base.Services.Interfaces;
using MriBase.App.Base.ViewModels;
using MriBase.App.Base.Views;
using MriBase.Models;
using MriBase.Models.Models;
using System.IO;
using System.Net;
using System.Reflection;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xfx;
using Platform = Xamarin.Essentials.Platform;

namespace MriBase.App.Dog.Droid
{
    [Activity(Label = "MriDogApp", Icon = "@drawable/AppIcon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private ITouchscreenCalibrationService touchCalibrationServce;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            XfxControls.Init();

            CheckPermissions();

            LoadApplication(new App());

            this.touchCalibrationServce = BaseViewModel.Container.Resolve<ITouchscreenCalibrationService>();
        }

        public void UpdateApp(string uri)
        {
            var webClient = new WebClient();

            var downloadPath = Path.Combine(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDownloads).AbsolutePath, "MriDogApp.apk");

            webClient.DownloadFileCompleted += (s, e) =>
            {
                if (e.Error is null)
                {
                    if (!PackageManager.CanRequestPackageInstalls())
                    {
                        var unknownAppSourceIntent = new Intent()
                        .SetAction("android.settings.MANAGE_UNKNOWN_APP_SOURCES")
                        .SetData(Uri.Parse($"package:{AppInfo.PackageName}"));
                        StartActivity(unknownAppSourceIntent);
                    }
                    else
                    {
                        InstallPackage(downloadPath);
                    }

                }
            };

            var url = new System.Uri(uri);

            webClient.DownloadFileAsync(url, downloadPath);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
        }

        private void InstallPackage(string downloadPath)
        {
            var promptInstall = new Intent(Intent.ActionView).SetDataAndType(FileProvider.GetUriForFile(this, $"{AppInfo.PackageName}.fileprovider", new Java.IO.File(downloadPath)), "application/vnd.android.package-archive");
            promptInstall.AddFlags(ActivityFlags.ClearTop);
            promptInstall.AddFlags(ActivityFlags.GrantReadUriPermission);
            StartActivity(promptInstall);
        }

        private void CheckPermissions()
        {
            string[] permissions = new[]
            {
                Manifest.Permission.ReadExternalStorage,
                Manifest.Permission.WriteExternalStorage,
                Manifest.Permission.RequestInstallPackages
            };

            bool minimumPermissionsGranted = true;

            foreach (string permission in permissions)
            {
                if (CheckSelfPermission(permission) != Permission.Granted)
                {
                    minimumPermissionsGranted = false;
                }
            }

            // If any of the minimum permissions aren't granted, we request them from the user
            if (!minimumPermissionsGranted)
            {
                RequestPermissions(permissions, 0);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override bool DispatchTouchEvent(MotionEvent motionEvent)
        {
            if (touchCalibrationServce.IsCalibrated && !(Xamarin.Forms.Application.Current.MainPage is NavigationPage navPage && navPage.CurrentPage is TouchscreenCalibrationPage))
            {
                int pointerIndex = motionEvent.ActionIndex;

                Point screenPointerCoords = new Point(motionEvent.GetX(pointerIndex),
                                                      motionEvent.GetY(pointerIndex));

                var point = touchCalibrationServce.TranslatePoint(new Point(this.BaseContext.FromPixels(screenPointerCoords.X), this.BaseContext.FromPixels(screenPointerCoords.Y)));

                motionEvent.SetLocation((float)this.BaseContext.ToPixels(point.X), (float)this.BaseContext.ToPixels(point.Y));
            }

            try
            {
                if (Xamarin.Forms.Application.Current.MainPage is NavigationPage naviPage && naviPage.CurrentPage is BaseTrainingPage baseTrainingPage)
                {
                    int pointerIndex = motionEvent.ActionIndex;
                    int pointerId = motionEvent.GetPointerId(pointerIndex);
                    var fraudDetectionData = baseTrainingPage.ViewModel.Result.FraudDetectionData;

                    var xPosition = motionEvent.GetX(pointerIndex);
                    var yPosition = motionEvent.GetY(pointerIndex);

                    var timeStamp = motionEvent.EventTime;

                    var test = motionEvent.GetToolType(pointerIndex);

                    var size = motionEvent.GetSize(pointerIndex);
                    var orientation = motionEvent.GetOrientation(pointerIndex);
                    var pressure = motionEvent.GetPressure(pointerIndex);
                    var toolMajor = motionEvent.GetToolMajor(pointerIndex);
                    var toolMinor = motionEvent.GetToolMinor(pointerIndex);
                    var touchMajor = motionEvent.GetTouchMajor(pointerIndex);
                    var touchMinor = motionEvent.GetTouchMinor(pointerIndex);
                    var eventAction = motionEvent.Action;

                    if (!baseTrainingPage.ViewModel.TrainingEnded && (motionEvent.Action == MotionEventActions.Down ||
                        motionEvent.Action == MotionEventActions.PointerDown ||
                        motionEvent.Action == MotionEventActions.Pointer1Down ||
                        motionEvent.Action == MotionEventActions.Pointer2Down ||
                        motionEvent.Action == MotionEventActions.Pointer3Down ||
                        motionEvent.Action == (MotionEventActions)773 ||
                        motionEvent.Action == (MotionEventActions)1029 ||
                        motionEvent.Action == (MotionEventActions)1285 ||
                        motionEvent.Action == (MotionEventActions)1541 ||
                        motionEvent.Action == (MotionEventActions)1797 ||
                        motionEvent.Action == (MotionEventActions)2053 ||
                        motionEvent.Action == (MotionEventActions)2309))
                    {
                        var motion = new TouchscreenMotion((int)eventAction, motionEvent.PointerCount, xPosition, yPosition, timeStamp, size, orientation, pressure, toolMajor, toolMinor, touchMajor, touchMinor, baseTrainingPage.ViewModel.Result.Trials.Count - 1);

                        fraudDetectionData.Motions.Add(motion);

                        fraudDetectionData.ActivePointers.Add(pointerId, motion);
                    }

                    var historySize = motionEvent.HistorySize;
                    for (int h = 0; h < historySize; h++)
                    {
                        for (int p = 0; p < motionEvent.PointerCount; p++)
                        {
                            var historyPointerId = motionEvent.GetPointerId(p);

                            var historicalSize = motionEvent.GetHistoricalSize(p, h);
                            var historicalOrientation = motionEvent.GetHistoricalOrientation(p, h);
                            var historicalPressure = motionEvent.GetHistoricalPressure(p, h);
                            var historicalX = motionEvent.GetHistoricalX(p, h);
                            var historicalY = motionEvent.GetHistoricalY(p, h);
                            var historicalTime = motionEvent.GetHistoricalEventTime(h);

                            var historicalToolMajor = motionEvent.GetHistoricalToolMajor(p, h);
                            var historicalToolMinor = motionEvent.GetHistoricalToolMinor(p, h);
                            var historicalTouchMajor = motionEvent.GetHistoricalTouchMajor(p, h);
                            var historicalTouchMinor = motionEvent.GetHistoricalTouchMinor(p, h);

                            if (fraudDetectionData.ActivePointers.TryGetValue(historyPointerId, out var pointer))
                            {
                                pointer.AddIntermediatePosition((int)eventAction, motionEvent.PointerCount, historicalX, historicalY, historicalTime, historicalSize, historicalOrientation, historicalPressure, historicalToolMajor, historicalToolMinor, historicalTouchMajor, historicalTouchMinor);
                            }
                        }
                    }
                    if (motionEvent.Action != MotionEventActions.Down &&
                        motionEvent.Action != MotionEventActions.Up &&
                        motionEvent.Action != MotionEventActions.Cancel &&
                        motionEvent.Action != MotionEventActions.PointerDown &&
                        motionEvent.Action != MotionEventActions.Pointer1Down &&
                        motionEvent.Action != MotionEventActions.Pointer2Down &&
                        motionEvent.Action != MotionEventActions.Pointer3Down &&
                        motionEvent.Action != MotionEventActions.PointerUp &&
                        motionEvent.Action != MotionEventActions.Pointer1Up &&
                        motionEvent.Action != MotionEventActions.Pointer2Up &&
                        motionEvent.Action != MotionEventActions.Pointer3Up &&
                        motionEvent.Action != (MotionEventActions)773 &&
                        motionEvent.Action != (MotionEventActions)774 &&
                        motionEvent.Action != (MotionEventActions)1029 &&
                        motionEvent.Action != (MotionEventActions)1030 &&
                        motionEvent.Action != (MotionEventActions)1285 &&
                        motionEvent.Action != (MotionEventActions)1286 &&
                        motionEvent.Action != (MotionEventActions)1541 &&
                        motionEvent.Action != (MotionEventActions)1542 &&
                        motionEvent.Action != (MotionEventActions)1797 &&
                        motionEvent.Action != (MotionEventActions)1798 &&
                        motionEvent.Action != (MotionEventActions)2053 &&
                        motionEvent.Action != (MotionEventActions)2054 &&
                        motionEvent.Action != (MotionEventActions)2309 &&
                        motionEvent.Action != (MotionEventActions)2310)
                    {
                        if (fraudDetectionData.ActivePointers.TryGetValue(pointerId, out var pointer))
                        {
                            pointer.AddIntermediatePosition((int)eventAction, motionEvent.PointerCount, xPosition, yPosition, timeStamp, size, orientation, pressure, toolMajor, toolMinor, touchMajor, touchMinor);
                        }
                    }

                    if (motionEvent.Action == MotionEventActions.Up || motionEvent.Action == MotionEventActions.Cancel ||
                        motionEvent.Action == MotionEventActions.PointerUp ||
                        motionEvent.Action == MotionEventActions.Pointer1Up ||
                        motionEvent.Action == MotionEventActions.Pointer2Up ||
                        motionEvent.Action == MotionEventActions.Pointer3Up ||
                        motionEvent.Action == (MotionEventActions)774 ||
                        motionEvent.Action == (MotionEventActions)1030 ||
                        motionEvent.Action == (MotionEventActions)1286 ||
                        motionEvent.Action == (MotionEventActions)1542 ||
                        motionEvent.Action == (MotionEventActions)1798 ||
                        motionEvent.Action == (MotionEventActions)2054 ||
                        motionEvent.Action == (MotionEventActions)2310)
                    {
                        if (fraudDetectionData.ActivePointers.TryGetValue(pointerId, out var pointer))
                        {
                            pointer.SetEndPosition((int)eventAction, motionEvent.PointerCount, xPosition, yPosition, timeStamp, size, orientation, pressure, toolMajor, toolMinor, touchMajor, touchMinor);

                            fraudDetectionData.ActivePointers.Remove(pointerId);
                        }
                    }

                }
            }
            catch (System.Exception)
            {
            }

            return base.DispatchTouchEvent(motionEvent);
        }
    }
}