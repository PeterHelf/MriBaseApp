namespace MriBase.Models.Models
{
    public class ProjectInfo
    {
        public ProjectInfo(string projectOwnerName, ProjectName projectName, string description, int projectId)
        {
            ProjectOwnerName = projectOwnerName;
            ProjectName = projectName;
            Description = description;
            ProjectId = projectId;
        }

        public string ProjectOwnerName { get; }

        public int ProjectId { get; }

        public ProjectName ProjectName { get; }

        public string Description { get; }
    }
}
