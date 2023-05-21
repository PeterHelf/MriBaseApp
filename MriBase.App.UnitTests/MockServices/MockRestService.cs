using Microsoft.Extensions.Logging;
using MriBase.Models.Enums;
using MriBase.Models.Interfaces;
using MriBase.Models.Models;
using MriBase.Models.Requests;
using MriBase.Models.Response;
using MriBase.Models.Services.Interfaces;
using MriBase.Models.WebFormData;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MriBase.App.UnitTests.MockServices
{
    internal class MockRestService : IRestService
    {
        private readonly ILogger<MockRestService> logger;

        public MockRestService(ILogger<MockRestService> logger)
        {
            this.logger = logger;
        }

        public string CurrentJwToken => throw new NotImplementedException();

        public string RefreshJwToken => throw new NotImplementedException();

        public Task ActvateQuestionnaireData(QuestionnaireFormData formData, int questionnaireId)
        {
            throw new NotImplementedException();
        }

        public Task<AddAnimalResponse> AddAnimal(int userId, DogInformation animal)
        {
            throw new NotImplementedException();
        }

        public Task AddAnimalToProject(int projectId, int animalId)
        {
            throw new NotImplementedException();
        }

        public Task<AddAnimalResponse> AddGoffin(BirdInformation animal)
        {
            throw new NotImplementedException();
        }

        public Task<AddAnimalResponse> AddKea(BirdInformation animal)
        {
            throw new NotImplementedException();
        }

        public Task AddMultipleStatistic(int animalId, List<int> trainingIds)
        {
            throw new NotImplementedException();
        }

        public Task<AddProjectResponse> AddProject(string projectDescription, ProjectName projectName, List<int> projectGroups)
        {
            throw new NotImplementedException();
        }

        public Task AddProjectFile(int projectId, string fileName, string fileExtension, Stream stream, long contentLength)
        {
            throw new NotImplementedException();
        }

        public Task AddProjectImage(int projectId, string imageFileName, string imageFileExtension, byte[] imageData)
        {
            throw new NotImplementedException();
        }

        public Task AddProjectLink(int projectId, string uri, string linkAlias)
        {
            throw new NotImplementedException();
        }

        public Task AddResearcherToProject(int projectId, string username)
        {
            throw new NotImplementedException();
        }

        public Task AddStatistic(int animalId, int trainingId)
        {
            throw new NotImplementedException();
        }

        public Task AddTraining(int projectId, Training training)
        {
            throw new NotImplementedException();
        }

        public Task AddTraining(Training training)
        {
            throw new NotImplementedException();
        }

        public Task<CreateGroupResponse> AdminCreateGroup(AdminCreateGroupRequest createRequest)
        {
            throw new NotImplementedException();
        }

        public Task<CreateUserResponse> AdminCreateUser(AdminCreateUserRequest createRequest)
        {
            throw new NotImplementedException();
        }

        public Task<GroupDeletionError> AdminDeleteGroup(int groupId)
        {
            throw new NotImplementedException();
        }

        public Task<List<GroupInfo>> AdminGetGroups()
        {
            throw new NotImplementedException();
        }

        public Task<AdminUserInfo> AdminGetUser(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserInfo>> AdminGetUsers()
        {
            throw new NotImplementedException();
        }

        public Task AdminUpdateUser(AdminUpdateUserRequest updateRequest)
        {
            throw new NotImplementedException();
        }

        public Task AdminUpdateUser(int userId, AdminUpdateUserRequest updateRequest)
        {
            throw new NotImplementedException();
        }

        public Task<AddProjectResponse> CloneProject(int projectId)
        {
            throw new NotImplementedException();
        }

        public Task CreateCsv()
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> CreateDatabaseBackup()
        {
            throw new NotImplementedException();
        }

        public Task DeactivateTraining(int projectId, int trainingId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteImage(string resourceUri)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProjectFile(int projectId, string fileName, string fileExtension)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProjectLink(int projectId, string uri, string linkAlias)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> DownloadFile(string resourcePath)
        {
            throw new NotImplementedException();
        }

        public Task<List<QuestionnaireFormDataInfo>> GetActivatedQuestionnaireData()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IAnimalInformation>> GetAnimals(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProjectInfo>> GetCdlProjects()
        {
            throw new NotImplementedException();
        }

        public Task<List<ProjectInfo>> GetCdlProjects(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<(List<Training> newAndUpdatedTrainings, List<int> DeletedTrainings)> GetCDLTrainings((int, DateTime)[] savedTrainings)
        {
            throw new NotImplementedException();
        }

        public Task<List<TrainingSessionResult>> GetFraudDetectionData(int trainingId)
        {
            throw new NotImplementedException();
        }

        public Task<List<TrainingSessionResult>> GetFraudDetectionData()
        {
            throw new NotImplementedException();
        }

        public Task<List<ProjectInfo>> GetGoffinProjects()
        {
            throw new NotImplementedException();
        }

        public Task<List<ProjectInfo>> GetGoffinProjects(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IAnimalInformation>> GetGoffins()
        {
            throw new NotImplementedException();
        }

        public Task<(List<Training> newAndUpdatedTrainings, List<int> DeletedTrainings)> GetGoffinTrainings((int, DateTime)[] savedTrainings)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserInfo>> GetGroupMembers(int groupId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProjectInfo>> GetGroupProjects(int groupId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProjectInfo>> GetGroupProjects(int groupId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetGroupRegisterToken(int groupId)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> GetImage(string resourceUri)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProjectInfo>> GetKeaProjects()
        {
            throw new NotImplementedException();
        }

        public Task<List<ProjectInfo>> GetKeaProjects(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IAnimalInformation>> GetKeas()
        {
            throw new NotImplementedException();
        }

        public Task<(List<Training> newAndUpdatedTrainings, List<int> DeletedTrainings)> GetKeaTrainings((int, DateTime)[] savedTrainings)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectData> GetProjectData(int projectId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProjectInfo>> GetProjects()
        {
            throw new NotImplementedException();
        }

        public Task<List<ProjectInfo>> GetProjects(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<QuestionnaireFormDataInfo>> GetQuestionnaireData()
        {
            throw new NotImplementedException();
        }

        public Task<QuestionnaireFormDataInfo> GetQuestionnaireDataById(int questionnaireId)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetQuestionnaireToken()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRegisterToken()
        {
            throw new NotImplementedException();
        }

        public Task<List<UserInfo>> GetResearchers()
        {
            throw new NotImplementedException();
        }

        public Task<TrainingStatisticDetails[]> GetStatisticDetails(int animalId, int trainingId)
        {
            throw new NotImplementedException();
        }

        public Task<Training> GetTraining(int trainingId)
        {
            throw new NotImplementedException();
        }

        public Task<(List<Training> newAndUpdatedTrainings, List<int> DeletedTrainings)> GetTrainings((int, DateTime)[] savedTrainings)
        {
            throw new NotImplementedException();
        }

        public Task<List<QuestionnaireFormDataInfo>> GetUnactivatedQuestionnaireData()
        {
            throw new NotImplementedException();
        }

        public Task<LoginResponse> LoginUser(LoginRequest loginRequest)
        {
            throw new NotImplementedException();
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        public void PauseTokenRefresh()
        {
            throw new NotImplementedException();
        }

        public Task ReactivateTraining(int projectId, int trainingId)
        {
            throw new NotImplementedException();
        }

        public void RefreshToken()
        {
            throw new NotImplementedException();
        }

        public Task<RegisterResponse> RegisterUser(RegisterRequest registerRequest)
        {
            throw new NotImplementedException();
        }

        public Task<RegisterResponse> RegisterWithLink(string registerToken, RegisterRequest registerRequest)
        {
            throw new NotImplementedException();
        }

        public Task RemoveResearcherFromProject(int projectId, string username)
        {
            throw new NotImplementedException();
        }

        public Task ResetPassword(string username)
        {
            throw new NotImplementedException();
        }

        public void ResumeTokenRefresh()
        {
            throw new NotImplementedException();
        }

        public Task SaveEthicsApplication(int projectId, string fileName, string fileExtension, Stream stream, long contentLength)
        {
            throw new NotImplementedException();
        }

        public Task SavePublication(int projectId, string fileName, string fileExtension, Stream stream, long contentLength)
        {
            throw new NotImplementedException();
        }

        public Task<List<QuestionnaireFormDataInfo>> SearchDogsAndDogOwners(string searchString = "*")
        {
            throw new NotImplementedException();
        }

        public Task<List<BirdInformation>> SearchGoffins(string searchString = "*")
        {
            throw new NotImplementedException();
        }

        public Task<List<BirdInformation>> SearchKeas(string searchString = "*")
        {
            throw new NotImplementedException();
        }

        public Task UpdateAnimal(int animalId, string newName, byte[] newImage)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAnimal(int animalId, string newName, byte[] newImage, DateTime newDateOfBirth, Gender newSex, Breed? newBreed)
        {
            throw new NotImplementedException();
        }

        public Task UpdateGroupMembers(int groupId, Dictionary<int, GroupRole> updateData)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProjectDescription(int projectId, string newDescription)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTraining(int projectId, int trainingId, Training training)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateUserResponse> UpdateUser(UpdateUserRequest userData)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateUserPasswordResponse> UpdateUserPassword(UpdateUserPasswordRequest userData)
        {
            throw new NotImplementedException();
        }

        public Task UpdloadExplanationVideo(int trainingId, byte[] videoData)
        {
            throw new NotImplementedException();
        }

        public Task UploadBackup(byte[] backup)
        {
            throw new NotImplementedException();
        }

        public Task UploadFile(string resourcePath, string fileName, Stream fileStream, Method method, long contentLength)
        {
            throw new NotImplementedException();
        }

        public Task UploadQuestionnaire(QuestionnaireFormData formData)
        {
            throw new NotImplementedException();
        }

        public Task UploadResult(TrainingSessionResult result)
        {
            logger.LogDebug($"Result from training with ID:{result.TrainingId} uploaded");

            return Task.CompletedTask;
        }

        Task<List<GroupRoleUserInfo>> IRestService.GetGroupMembers(int groupId)
        {
            throw new NotImplementedException();
        }
    }
}