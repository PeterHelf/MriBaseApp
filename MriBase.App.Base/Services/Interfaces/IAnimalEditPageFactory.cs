using MriBase.App.Base.Views;
using MriBase.Models.Interfaces;

namespace MriBase.App.Base.Services.Interfaces
{
    public interface IAnimalEditPageFactory : IFactory<AnimalEditPageBase, IAnimalInformation>
    {
    }
}