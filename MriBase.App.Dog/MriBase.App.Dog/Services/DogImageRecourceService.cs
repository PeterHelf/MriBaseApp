using MriBase.Models.Services.Interfaces;
using System.IO;
using System.Reflection;

namespace MriBase.App.Dog.Implementations
{
    public class DogImageRecourceService : IImageRecourceService
    {
        public byte[] GetImage(string imageName)
        {
            var assembly = Assembly.Load(new AssemblyName("MriBase.Models"));
            var stream = assembly.GetManifestResourceStream($"MriBase.Models.Resources.Images.{imageName}");

            using (var ms = new MemoryStream())
            {
                stream?.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public byte[] GetDefaultAnimalImage()
        {
            return this.GetImage("defaultDogImage.png");
        }
    }
}