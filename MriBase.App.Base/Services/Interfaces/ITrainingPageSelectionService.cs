using MriBase.App.Base.Views;
using MriBase.Models.Models;

namespace MriBase.App.Base.Services.Interfaces
{
    public interface ITrainingPageSelectionService
    {
        BaseTrainingPage GetTrainingPage(Training training, bool startWithBluetooth = false);
    }
}