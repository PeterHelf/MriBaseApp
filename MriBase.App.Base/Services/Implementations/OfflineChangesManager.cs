using MriBase.App.Base.Services.Interfaces;
using MriBase.Models.Enums;
using MriBase.Models.Models;
using MriBase.Models.Services.Interfaces;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MriBase.App.Base.Services.Implementations
{
    public class OfflineChangesManager : IOfflineChangesManager
    {
        private readonly IAppDataService appDataService;
        private readonly ILocalSaveService localSaveService;

        private List<TrainingSessionResult> Results { get; set; }

        private List<int> ChangedAnimalIds { get; set; }

        public OfflineChangesManager(IAppDataService appDataService, ILocalSaveService localSaveService)
        {
            this.appDataService = appDataService ?? throw new ArgumentNullException(nameof(appDataService));
            this.localSaveService = localSaveService ?? throw new ArgumentNullException(nameof(localSaveService));
            this.Results = this.localSaveService.LoadOfflineResults();
            this.ChangedAnimalIds = this.localSaveService.LoadChangedAnimalIds();
        }

        public void AddResult(TrainingSessionResult result)
        {
            this.Results.Add(result);
            this.localSaveService.SaveOfflineResults(this.Results);
        }

        public void AddChangedAnimal(int animalId)
        {
            if (this.ChangedAnimalIds.Contains(animalId))
            {
                return;
            }

            this.ChangedAnimalIds.Add(animalId);
            this.localSaveService.SaveChangedAnimnalIds(this.ChangedAnimalIds);
        }
    }
}
