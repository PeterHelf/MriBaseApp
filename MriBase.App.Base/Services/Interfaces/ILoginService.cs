using MriBase.Models.Enums;
using System.Threading.Tasks;

namespace MriBase.App.Base.Services.Interfaces
{
    public interface ILoginService
    {
        Task<LoginError> LoginOffline(string userName, string passwordHash);
    }
}