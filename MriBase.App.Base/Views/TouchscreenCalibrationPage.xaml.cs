using MriBase.App.Base.Events.Arguments;
using MriBase.App.Base.Services.Implementations;
using MriBase.App.Base.Services.Interfaces;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MriBase.App.Base.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TouchscreenCalibrationPage : ContentPage
    {
        private readonly ITouchscreenCalibrationService touchscreenCalibrationService;
        private readonly INavigationService navigationService;
        private BitmapCreationService bmpMaker;

        private int calibrationIndex;

        public TouchscreenCalibrationPage(ITouchscreenCalibrationService touchscreenCalibrationService, INavigationService navigationService)
        {
            InitializeComponent();
            this.touchscreenCalibrationService = touchscreenCalibrationService ?? throw new ArgumentNullException(nameof(touchscreenCalibrationService));
            this.navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            this.touchscreenCalibrationService.ResetCalibration();
            this.touchscreenCalibrationService.CalibrationPoints.Clear();
            this.touchscreenCalibrationService.CalibrationPoints.Add(new Point(width * 0.03, height * 0.43));
            this.touchscreenCalibrationService.CalibrationPoints.Add(new Point(width * 0.55, height * 0.93));
            this.touchscreenCalibrationService.CalibrationPoints.Add(new Point(width * 0.89, height * 0.06));
            this.calibrationIndex = 0;

            DrawNextCalibrationPoint();
            base.OnSizeAllocated(width, height);
        }

        private void DrawNextCalibrationPoint()
        {
            if (calibrationIndex >= this.touchscreenCalibrationService.CalibrationPoints.Count)
            {
                touchscreenCalibrationService.SaveCalibration();
                navigationService.ReturnToLastPage();
                return;
            }

            this.bmpMaker = new BitmapCreationService(Convert.ToInt32(Math.Round(this.Width, MidpointRounding.AwayFromZero)), Convert.ToInt32(Math.Round(this.Height, MidpointRounding.AwayFromZero)));
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    try
                    {
                        bmpMaker.SetPixel(Convert.ToInt32(Math.Round(touchscreenCalibrationService.CalibrationPoints[calibrationIndex].Y)) - 2 + i, Convert.ToInt32(Math.Round(touchscreenCalibrationService.CalibrationPoints[calibrationIndex].X)) - 2 + j, Color.Red);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            ImageSource imageSource = bmpMaker.Generate();
            image.Source = imageSource;
        }

        void OnTouchEffectAction(object sender, CalibratedTouchActionEventArgs args)
        {
            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    touchscreenCalibrationService.CalibratePoint(args.Location);
                    this.calibrationIndex++;
                    DrawNextCalibrationPoint();
                    break;
                case TouchActionType.Moved:
                case TouchActionType.Released:
                case TouchActionType.Cancelled:
                    break;
            }
        }
    }
}