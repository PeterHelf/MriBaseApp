namespace MriBase.App.Base.Services.Interfaces
{
    public interface IContainer
    {
        TItem Resolve<TItem>();
    }
}