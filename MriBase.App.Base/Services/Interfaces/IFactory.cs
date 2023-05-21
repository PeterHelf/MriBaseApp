namespace MriBase.App.Base.Services.Interfaces
{
    public interface IFactory<T, U>
    {
        T CreateInstance(U item);
    }
}
