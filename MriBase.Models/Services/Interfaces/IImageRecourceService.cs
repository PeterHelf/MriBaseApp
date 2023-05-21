namespace MriBase.Models.Services.Interfaces
{
    public interface IImageRecourceService
    {
        byte[] GetDefaultAnimalImage();
        byte[] GetImage(string imageName);
    }
}