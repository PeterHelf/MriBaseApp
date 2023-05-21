using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MriBase.Models.Models
{
    [Serializable]
    public class TouchscreenMotion
    {
        public TouchscreenMotion(int action, int nrOfActivePointers, float startingX, float startingY, long timeStamp, float size, float orientation, float pressure, float toolMajor, float toolMinor, float touchMajor, float touchMinor, int trialNr)
        {
            this.StartingPosition = new TouchscreenMotionPoint(action, nrOfActivePointers, startingX, startingY, timeStamp, size, orientation, pressure, toolMajor, toolMinor, touchMajor, touchMinor);
            this.IntermediatePositions = new List<TouchscreenMotionPoint>();
            this.TrialNr = trialNr;
        }

        public TouchscreenMotion(TouchscreenMotionPoint startingPosition, TouchscreenMotionPoint endingPosition, List<TouchscreenMotionPoint> intermediatePositions, int trialNr)
        {
            this.StartingPosition = startingPosition;
            this.EndingPosition = endingPosition;
            this.IntermediatePositions = intermediatePositions;
            this.TrialNr = trialNr;
        }

        [JsonConstructor]
        private TouchscreenMotion()
        {

        }

        public int TrialNr { get; set; }

        public TouchscreenMotionPoint StartingPosition { get; set; }

        public List<TouchscreenMotionPoint> IntermediatePositions { get; set; }

        public TouchscreenMotionPoint EndingPosition { get; set; }

        public void SetEndPosition(int action, int nrOfActivePointers, float x, float y, long timeStamp, float size, float orientation, float pressure, float toolMajor, float toolMinor, float touchMajor, float touchMinor)
        {
            this.EndingPosition = new TouchscreenMotionPoint(action, nrOfActivePointers, x, y, timeStamp, size, orientation, pressure, toolMajor, toolMinor, touchMajor, touchMinor);
        }

        public void AddIntermediatePosition(int action, int nrOfActivePointers, float x, float y, long timeStamp, float size, float orientation, float pressure, float toolMajor, float toolMinor, float touchMajor, float touchMinor)
        {
            this.IntermediatePositions.Add(new TouchscreenMotionPoint(action, nrOfActivePointers, x, y, timeStamp, size, orientation, pressure, toolMajor, toolMinor, touchMajor, touchMinor));
        }
    }
}