namespace MriBase.Models.Models
{
    public class ProjectImage : ProjectFile
    {
        public ProjectImage(string fileName, string fileExtension, string fileUri) : base(fileName, fileExtension, fileUri)
        {
        }

        public string ImageDataBase64 { get; set; }
    }
}
