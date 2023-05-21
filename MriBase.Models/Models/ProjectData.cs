using System.Collections.Generic;

namespace MriBase.Models.Models
{
    public class ProjectData
    {
        public ProjectData(int id, string ownerName, string description, ProjectName projectName, List<string> editorUsernames, List<ProjectAnimals> animalTypes, List<TrainingInfos> trainings, ProjectFile publication, ProjectFile ethicsApplication, List<Link> additionalLinks, List<ProjectFile> additionalFiles, List<ProjectImage> images)
        {
            ProjectId = id;
            OwnerName = ownerName;
            Description = description;
            ProjectName = projectName;
            ProjectEditors = editorUsernames;
            AnimalTypes = animalTypes;
            Trainings = trainings;
            PublicationFile = publication;
            EthicsApplicationFile = ethicsApplication;
            AdditionalLinks = additionalLinks;
            AdditionalFiles = additionalFiles;
            Images = images;
        }

        public ProjectName ProjectName { get; set; }

        public List<TrainingInfos> Trainings { get; set; }

        public int ProjectId { get; set; }

        public string OwnerName { get; set; }

        public List<string> ProjectEditors { get; set; }

        public string Description { get; set; }

        public ProjectFile PublicationFile { get; set; }

        public ProjectFile EthicsApplicationFile { get; set; }

        public List<Link> AdditionalLinks { get; set; }

        public List<ProjectFile> AdditionalFiles { get; set; }

        public List<ProjectImage> Images { get; set; }

        public List<ProjectAnimals> AnimalTypes { get; set; }
    }
}
