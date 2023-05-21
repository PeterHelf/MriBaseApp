using MriBase.App.Base.Bluetooth;
using MriBase.App.Base.Services.Interfaces;
using MriBase.Models.Models;
using MriBase.Models.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Point = System.Drawing.Point;

namespace MriBase.App.Base.ViewModels
{
    public class RandomPositionTestViewModel : BaseTrainingViewModel
    {
        private IEnumerable<TrainingImageViewModel> currentImages;

        public IEnumerable<TrainingImageViewModel> CurrentImages
        {
            get => currentImages;
            set
            {
                currentImages = value;
                this.OnPropertyChanged();
            }
        }

        public RandomPositionTestViewModel(Training training, INavigationService navigationService, IOfflineChangesManager offlineChangesManager, IFeederService feederService, ILocalSaveService localSaveService, IAppDataService appDataService, IBluetoothGATTServer bluetoothGATTServer)
            : base(training, navigationService, offlineChangesManager, feederService, localSaveService, appDataService, bluetoothGATTServer)
        {
        }

        protected override void InitNextTrial(TrainingTrial trial)
        {
            this.CurrentImages = this.CreateImageVms(trial.Parts.First().Images);
        }

        private IEnumerable<TrainingImageViewModel> CreateImageVms(IEnumerable<TrainingImage> images)
        {
            var imgs = images.ToArray();

            var createdImgs = new List<TrainingImageViewModel>();

            for (int i = 0; i < imgs.Count(); i++)
            {
                var vm = new TrainingImageViewModel(imgs[i]);

                Point point;
                do
                {
                    point = new Point(Rnd.Next(6), Rnd.Next(4));

                } while (createdImgs.Any(img => img.Position == point));

                vm.Position = point;

                createdImgs.Add(vm);
            }

            return createdImgs;
        }
    }
}