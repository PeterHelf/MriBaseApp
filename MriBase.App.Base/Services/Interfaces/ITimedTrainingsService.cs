using MriBase.Models.Models;

namespace MriBase.App.Base.Services.Interfaces
{
    public interface ITimedTrainingsService
    {
        void StartAllTimers();
        void StartTraining(TimedTraining training);
    }
}