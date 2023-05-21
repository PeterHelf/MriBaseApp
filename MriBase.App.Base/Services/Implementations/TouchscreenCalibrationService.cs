using MriBase.App.Base.Services.Interfaces;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MriBase.App.Base.Services.Implementations
{
    public class TouchscreenCalibrationService : ITouchscreenCalibrationService
    {
        public List<Point> CalibrationPoints { get; }

        public bool IsCalibrated => this.calibrationMatrix.Divider != 0;

        private readonly List<Point> clickedPoints;

        private Matrix calibrationMatrix;

        public TouchscreenCalibrationService()
        {
            this.CalibrationPoints = new List<Point>();

            this.clickedPoints = new List<Point>();

            this.calibrationMatrix = new Matrix();
        }

        public void ResetCalibration()
        {
            this.clickedPoints.Clear();
        }

        public void CalibratePoint(Point point)
        {
            if (this.clickedPoints.Count >= this.CalibrationPoints.Count)
            {
                return;
            }

            this.clickedPoints.Add(point);
        }

        public Point TranslatePoint(Point point)
        {
            if (this.calibrationMatrix.Divider == 0)
            {
                return point;
            }

            var x = ((calibrationMatrix.Xa * point.X) +
                     (calibrationMatrix.Xb * point.Y) +
                      calibrationMatrix.Xc) /
                      calibrationMatrix.Divider;

            var y = ((calibrationMatrix.Ya * point.X) +
                     (calibrationMatrix.Yb * point.Y) +
                      calibrationMatrix.Yc) /
                      calibrationMatrix.Divider;

            return new Point(x, y);
        }

        public void SaveCalibration()
        {
            this.calibrationMatrix.Divider = Convert.ToInt32(((clickedPoints[0].X - clickedPoints[2].X) * (clickedPoints[1].Y - clickedPoints[2].Y)) -
                                                             ((clickedPoints[1].X - clickedPoints[2].X) * (clickedPoints[0].Y - clickedPoints[2].Y)));

            if (this.calibrationMatrix.Divider == 0)
            {
                throw new InvalidOperationException();
            }

            this.calibrationMatrix.Xa = Convert.ToInt32(((CalibrationPoints[0].X - CalibrationPoints[2].X) * (clickedPoints[1].Y - clickedPoints[2].Y)) -
                                                        ((CalibrationPoints[1].X - CalibrationPoints[2].X) * (clickedPoints[0].Y - clickedPoints[2].Y)));

            this.calibrationMatrix.Xb = Convert.ToInt32(((clickedPoints[0].X - clickedPoints[2].X) * (CalibrationPoints[1].X - CalibrationPoints[2].X)) -
                                                        ((CalibrationPoints[0].X - CalibrationPoints[2].X) * (clickedPoints[1].X - clickedPoints[2].X)));

            this.calibrationMatrix.Xc = Convert.ToInt32((clickedPoints[2].X * CalibrationPoints[1].X - clickedPoints[1].X * CalibrationPoints[2].X) * clickedPoints[0].Y +
                                                        (clickedPoints[0].X * CalibrationPoints[2].X - clickedPoints[2].X * CalibrationPoints[0].X) * clickedPoints[1].Y +
                                                        (clickedPoints[1].X * CalibrationPoints[0].X - clickedPoints[0].X * CalibrationPoints[1].X) * clickedPoints[2].Y);

            this.calibrationMatrix.Ya = Convert.ToInt32(((CalibrationPoints[0].Y - CalibrationPoints[2].Y) * (clickedPoints[1].Y - clickedPoints[2].Y)) -
                                                        ((CalibrationPoints[1].Y - CalibrationPoints[2].Y) * (clickedPoints[0].Y - clickedPoints[2].Y)));

            this.calibrationMatrix.Yb = Convert.ToInt32(((clickedPoints[0].X - clickedPoints[2].X) * (CalibrationPoints[1].Y - CalibrationPoints[2].Y)) -
                                                        ((CalibrationPoints[0].Y - CalibrationPoints[2].Y) * (clickedPoints[1].X - clickedPoints[2].X)));

            this.calibrationMatrix.Yc = Convert.ToInt32((clickedPoints[2].X * CalibrationPoints[1].Y - clickedPoints[1].X * CalibrationPoints[2].Y) * clickedPoints[0].Y +
                                                        (clickedPoints[0].X * CalibrationPoints[2].Y - clickedPoints[2].X * CalibrationPoints[0].Y) * clickedPoints[1].Y +
                                                        (clickedPoints[1].X * CalibrationPoints[0].Y - clickedPoints[0].X * CalibrationPoints[1].Y) * clickedPoints[2].Y);
        }
    }

    public class Matrix
    {
        public int Xa { get; set; }
        public int Xb { get; set; }
        public int Xc { get; set; }
        public int Ya { get; set; }
        public int Yb { get; set; }
        public int Yc { get; set; }
        public int Divider { get; set; }
    }
}
