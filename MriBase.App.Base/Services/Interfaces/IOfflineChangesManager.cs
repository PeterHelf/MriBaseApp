using MriBase.Models.Models;
using System.Threading.Tasks;

namespace MriBase.App.Base.Services.Interfaces
{
    public interface IOfflineChangesManager
    {
        void AddChangedAnimal(int animalId);
        void AddResult(TrainingSessionResult result);
    }
}