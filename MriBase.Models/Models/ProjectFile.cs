namespace MriBase.Models.Models
{
    public class ProjectFile
    {
        public ProjectFile(string fileName, string fileExtension, string fileUri)
        {
            FileName = fileName;
            FileExtension = fileExtension;
            FileUri = fileUri;
        }

        public string FileName { get; set; }

        public string FileExtension { get; set; }

        public string FileUri { get; set; }
    }
}
