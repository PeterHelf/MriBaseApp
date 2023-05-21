using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MriBase.Models.Models
{
    [Serializable]
    public class TouchscreenMotionPoint
    {
        public TouchscreenMotionPoint(int action, int nrOfActivePointers, float xPosition, float yPosition, long timeStamp, float size, float orientation, float pressure, float toolMajor, float toolMinor, float touchMajor, float touchMinor)
        {
            this.Action = action;
            this.NrOfActivePointers = nrOfActivePointers;
            this.XPosition = xPosition;
            this.YPosition = yPosition;
            this.TimeStamp = timeStamp;
            this.Size = size;
            this.Orientation = orientation;
            this.Pressure = pressure;
            this.ToolMajor = toolMajor;
            this.ToolMinor = toolMinor;
            this.TouchMajor = touchMajor;
            this.TouchMinor = touchMinor;
        }

        public int Action { get; }

        public int NrOfActivePointers { get; }

        public float XPosition { get; }

        public float YPosition { get; }

        public long TimeStamp { get; }

        public float Size { get; }

        public float Orientation { get; }

        public float Pressure { get; }

        public float ToolMajor { get; }
        public float ToolMinor { get; }
        public float TouchMajor { get; }
        public float TouchMinor { get; }
    }
}
