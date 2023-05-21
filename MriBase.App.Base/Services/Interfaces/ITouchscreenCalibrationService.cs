using System.Collections.Generic;
using Xamarin.Forms;

namespace MriBase.App.Base.Services.Interfaces
{
    public interface ITouchscreenCalibrationService
    {
        List<Point> CalibrationPoints { get; }
        bool IsCalibrated { get; }

        void ResetCalibration();
        void CalibratePoint(Point point);
        void SaveCalibration();
        Point TranslatePoint(Point point);
    }
}
