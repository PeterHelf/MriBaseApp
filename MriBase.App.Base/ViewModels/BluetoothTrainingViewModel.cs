using MriBase.App.Base.Bluetooth;
using MriBase.Models.EventArgs;
using MriBase.Models.Models;
using MriBase.Models.Translation;
using System;

namespace MriBase.App.Base.ViewModels
{
    public class BluetoothTrainingViewModel : BaseViewModel
    {
        private readonly Training training;
        private readonly IBluetoothService bluetoothService;
        private int trialNr;
        private bool sessionEnded;
        private int totalErrors;
        private double totalDuration;
        private double lastTrialDuration;
        private bool lastTrialCorrect;
        private bool isInCorrectiontrial;
        private bool noResults;
        private int totalCorrect;

        public string TrainingName => Translator.Instance.TranslateText(this.training.Name);

        public bool SessionEnded
        {
            get => sessionEnded;
            private set
            {
                sessionEnded = value;
                this.OnPropertyChanged();
            }
        }

        public int TrialNr
        {
            get => trialNr;
            private set
            {
                trialNr = value;
                this.OnPropertyChanged();
            }
        }

        public int TotalErrors
        {
            get => totalErrors;
            private set
            {
                totalErrors = value;
                this.OnPropertyChanged();
            }
        }
        public int TotalCorrect
        {
            get => totalCorrect;
            private set
            {
                totalCorrect = value;
                this.OnPropertyChanged();
            }
        }

        public double TotalDuration
        {
            get => Math.Round(totalDuration, 1);
            private set
            {
                totalDuration = value;
                this.OnPropertyChanged();
            }
        }

        public double LastTrialDuration
        {
            get => Math.Round(lastTrialDuration, 1);
            private set
            {
                lastTrialDuration = value;
                this.OnPropertyChanged();
            }
        }

        public bool LastTrialCorrect
        {
            get => lastTrialCorrect && !this.SessionEnded;
            private set
            {
                lastTrialCorrect = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(LastTrialIncorrect));
            }
        }

        public bool LastTrialIncorrect => !this.LastTrialCorrect && !this.NoResults && !this.SessionEnded;

        public bool NoResults
        {
            get => noResults && !this.SessionEnded;
            private set
            {
                noResults = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsInCorrectiontrial
        {
            get => isInCorrectiontrial;
            private set
            {
                isInCorrectiontrial = value;
                this.OnPropertyChanged();
            }
        }

        public BluetoothTrainingViewModel(Training training, IBluetoothService bluetoothService)
        {
            this.trialNr = 1;
            this.NoResults = true;
            this.training = training;
            this.bluetoothService = bluetoothService;
            this.bluetoothService.BluetoothMessageReceived += BluetoothManagerBluetoothMessageReceived;
        }

        private void BluetoothManagerBluetoothMessageReceived(object sender, BluetoothMessageReceivedEventArgs e)
        {
            this.SessionEnded = e.BluetoothMessage.IsSessionEnd;
            this.NoResults = false;

            if (this.SessionEnded)
            {
                this.TotalDuration = e.BluetoothMessage.Duration.TotalSeconds;
                this.OnPropertyChanged(null);
                return;
            }
            else
            {
                this.LastTrialDuration = e.BluetoothMessage.Duration.TotalSeconds;
            }

            if (e.BluetoothMessage.TrialCorrect || !training.SessionSettings.CorrectionTrialsActive)
            {
                this.TrialNr++;
                this.IsInCorrectiontrial = false;
            }
            else
            {
                this.IsInCorrectiontrial = true;
            }

            if (!e.BluetoothMessage.TrialCorrect)
            {
                this.TotalErrors++;
                this.LastTrialCorrect = false;
            }
            else
            {
                this.TotalCorrect++;
                this.LastTrialCorrect = true;
            }
        }
    }
}
