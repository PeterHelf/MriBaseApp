using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MriBase.Models.Models
{
    [Serializable]
    public class FraudDetectionData
    {
        public FraudDetectionData(string deviceType, string deviceModel, double deviceHeight, double deviceWidth)
        {
            this.Motions = new List<TouchscreenMotion>();
            this.DeviceType = deviceType;
            this.DeviceModel = deviceModel;
            this.DeviceHeight = deviceHeight;
            this.DeviceWidth = deviceWidth;

            this.ActivePointers = new Dictionary<int, TouchscreenMotion>();
        }

        public string DeviceType { get; }

        public string DeviceModel { get; }

        public double DeviceHeight { get; }

        public double DeviceWidth { get; }

        public List<TouchscreenMotion> Motions { get; }

        public Dictionary<int, TouchscreenMotion> ActivePointers { get; }
    }
}
