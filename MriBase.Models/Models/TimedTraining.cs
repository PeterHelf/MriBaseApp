using MriBase.Models.Enums;
using MriBase.Models.Interfaces;
using System;
using System.Timers;

namespace MriBase.Models.Models
{
    [Serializable]
    public class TimedTraining
    {
        public TimeSpan StartTime { get; set; }

        public bool AnyTraining { get; set; }

        public int SpecificTrainingId { get; set; }

        public int AnimalId { get; set; }

        [NonSerialized]
        private Timer timer;

        public TimedTraining(TimeSpan time, bool anyTraining, int specificTrainingId, int animalId)
        {
            StartTime = time;
            AnyTraining = anyTraining;
            SpecificTrainingId = specificTrainingId;
            AnimalId = animalId;
        }

        public void StopTimer()
        {
            this.timer.Stop();
            this.timer.Dispose();
        }

        public void StartTimer(Action<TimedTraining> startTraining)
        {
            if (!(this.timer is null) && this.timer.Enabled)
            {
                return;
            }

            TimeSpan now = DateTime.Now.TimeOfDay;

            double additionalDayMilliSeconds = 0;

            if (now > StartTime)
            {
                additionalDayMilliSeconds = new TimeSpan(1, 0, 0, 0).TotalMilliseconds;
            }

            int msUntilStart = (int)((StartTime - now).TotalMilliseconds + additionalDayMilliSeconds);

            this.timer = new Timer(msUntilStart);
            this.timer.Elapsed += (sender, args) => startTraining(this);
            this.timer.AutoReset = false;
            this.timer.Start();
        }
    }
}