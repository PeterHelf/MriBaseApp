using Microcharts;
using MriBase.App.Base.Commands;
using MriBase.App.Base.ExtensionMethods;
using MriBase.App.Base.Services.Interfaces;
using MriBase.Models.Enums;
using MriBase.Models.Models;
using MriBase.Models.Resources;
using MriBase.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MriBase.App.Base.ViewModels
{
    public class TrainingStatisticDetailsViewModel : BaseViewModel
    {
        private TrainingStatisticDetails[] statisticDetails;
        private IEnumerable<ChartEntry> failureChartEntries;
        private IEnumerable<ChartEntry> timeChartEntries;
        private DateTime selectedDate;
        private GroupOptionViewModel groupOption;
        private readonly IAppDataService appDataService;
        private readonly IImageRecourceService imageRecourceService;

        public TrainingStatisticDetailsViewModel(TrainingStatistic statistic, IAppDataService appDataService, IImageRecourceService imageRecourceService)
        {
            this.appDataService = appDataService;
            this.imageRecourceService = imageRecourceService;
            this.selectedDate = DateTime.Today;
            this.groupOption = new GroupOptionViewModel(GroupOption.Month);

            this.StatisticVm = new TrainingsStatisticViewModel(statistic);

            this.AddYearCommand = new ExtendedCommand(() => this.SelectedDate = this.SelectedDate.AddYears(1), () => this.SelectedDate.AddYears(1) <= DateTime.Today);
            this.SubtractYearCommand = new ExtendedCommand(() => this.SelectedDate = this.SelectedDate.AddYears(-1), () => this.SelectedDate.Year > 2020);

            this.AddMonthCommand = new ExtendedCommand(() => this.SelectedDate = this.SelectedDate.AddMonths(1), () => this.SelectedDate.AddMonths(1) <= DateTime.Today);
            this.SubtractMonthCommand = new ExtendedCommand(() => this.SelectedDate = this.SelectedDate.AddMonths(-1), () => this.SelectedDate.AddMonths(-1).Year >= 2020);

            this.AddDayCommand = new ExtendedCommand(() => this.SelectedDate = this.SelectedDate.AddDays(1), () => this.SelectedDate.AddDays(1) <= DateTime.Today);
            this.SubtractDayCommand = new ExtendedCommand(() => this.SelectedDate = this.SelectedDate.AddDays(-1), () => this.SelectedDate.AddDays(-1).Year >= 2020);

            this.statisticDetails = new TrainingStatisticDetails[0];

            Task.Run(() => Initialize());
        }

        private async void Initialize()
        {
            this.IsBusy = true;
            this.BusyText = ResViewStatistics.LoadingDetails;

            await Device.InvokeOnMainThreadAsync(async () =>
                await Application.Current.MainPage.DisplayAlert(ResViewStatistics.ServerUnavailable, ResViewStatistics.CouldNotLoadStatistics, ResViewBasics.Ok));

            this.UpdateCharts();
            this.IsBusy = false;
        }

        public TrainingsStatisticViewModel StatisticVm { get; }

        public IEnumerable<ChartEntry> FailureChartEntries
        {
            get => failureChartEntries;
            set
            {
                failureChartEntries = value;
                this.OnPropertyChanged();
            }
        }

        public IEnumerable<ChartEntry> TimeChartEntries
        {
            get => timeChartEntries;
            set
            {
                timeChartEntries = value;
                this.OnPropertyChanged();
            }
        }

        //HACK
        public void UpdateCharts()
        {
            this.Groups = GroupDetails();

            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            string labelStringFormat = "";
            string valueLabelStringFormat = "";
            Func<DateTime, DateTime> incrementDate = null;

            switch (this.SelectedGroupOption.GroupOption)
            {
                case GroupOption.Day:
                    startDate = this.SelectedDate;
                    endDate = this.SelectedDate.AddDays(1);
                    labelStringFormat = "HH";
                    valueLabelStringFormat = "#0.0";
                    incrementDate = d => d.AddHours(1);
                    break;
                case GroupOption.Month:
                    startDate = new DateTime(this.SelectedDate.Year, this.SelectedDate.Month, 1);
                    endDate = (new DateTime(this.SelectedDate.Year, this.SelectedDate.Month, 1)).AddMonths(1);
                    labelStringFormat = "dd";
                    valueLabelStringFormat = "#0.0";
                    incrementDate = d => d.AddDays(1);
                    break;
                case GroupOption.Year:
                    startDate = new DateTime(this.SelectedDate.Year, 1, 1);
                    endDate = (new DateTime(this.SelectedDate.Year, 1, 1)).AddYears(1);
                    labelStringFormat = "MMM";
                    valueLabelStringFormat = "#0.0";
                    incrementDate = d => d.AddMonths(1);
                    break;
                case GroupOption.SessionsDay:
                    valueLabelStringFormat = "#0.0";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(this.SelectedGroupOption));
            }

            var failureEntries = new List<ChartEntry>();
            var timeEntries = new List<ChartEntry>();

            var mostMistakes = this.Groups.Values.DefaultIfEmpty().Max(g => g.averageMistakes);
            var longestTime = this.Groups.Values.DefaultIfEmpty().Max(g => g.averageSeconds);

            string colorFunction(double current, double max)
            {
                if (double.IsNaN(current))
                {
                    return Color.Transparent.ToHex();
                }

                if (max == 0)
                {
                    return new Color(2.0f * current / 10, 2.0f * (1 - current / 10), 0).ToHex();
                }

                return new Color(2.0f * current / max, 2.0f * (1 - current / max), 0).ToHex();
            }

            switch (this.SelectedGroupOption.GroupOption)
            {
                case GroupOption.Day:
                case GroupOption.Month:
                case GroupOption.Year:
                    for (var day = startDate; day.Date < endDate; day = incrementDate(day))
                    {
                        (double averageMistakes, double averageSeconds) value = (double.NaN, double.NaN);

                        if (this.Groups.ContainsKey(day))
                        {
                            this.Groups.TryGetValue(day, out value);
                        }

                        var mistakesHexColorString = colorFunction(value.averageMistakes, mostMistakes);

                        failureEntries.Add(new ChartEntry(Convert.ToSingle(double.IsNaN(value.averageMistakes) || value.averageMistakes == 0 ? 0.01 : value.averageMistakes))
                        {
                            Label = day.ToString(labelStringFormat),
                            ValueLabel = Convert.ToSingle(value.averageMistakes).ToString(valueLabelStringFormat),
                            Color = SkiaSharp.SKColor.Parse(mistakesHexColorString)
                        });

                        var timeHexColorString = colorFunction(value.averageSeconds, longestTime);

                        timeEntries.Add(new ChartEntry(Convert.ToSingle(double.IsNaN(value.averageSeconds) || value.averageSeconds == 0 ? 0.01 : value.averageSeconds))
                        {
                            Label = day.ToString(labelStringFormat),
                            ValueLabel = Convert.ToSingle(value.averageSeconds).ToString(valueLabelStringFormat),
                            Color = SkiaSharp.SKColor.Parse(timeHexColorString)
                        });
                    }
                    break;
                case GroupOption.SessionsDay:
                    foreach (var stat in this.Groups)
                    {
                        var mistakesHexColorString = colorFunction(stat.Value.averageMistakes, mostMistakes);

                        failureEntries.Add(new ChartEntry(Convert.ToSingle(stat.Value.averageMistakes == 0 ? 0.01 : stat.Value.averageMistakes))
                        {
                            Label = string.Empty,
                            ValueLabel = Convert.ToSingle(stat.Value.averageMistakes).ToString(valueLabelStringFormat),
                            Color = SkiaSharp.SKColor.Parse(mistakesHexColorString)
                        });

                        var timeHexColorString = colorFunction(stat.Value.averageSeconds, longestTime);

                        timeEntries.Add(new ChartEntry(Convert.ToSingle(stat.Value.averageSeconds))
                        {
                            Label = string.Empty,
                            ValueLabel = Convert.ToSingle(stat.Value.averageSeconds).ToString(valueLabelStringFormat),
                            Color = SkiaSharp.SKColor.Parse(timeHexColorString)
                        });
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(this.SelectedGroupOption));
            }

            this.FailureChartEntries = failureEntries;
            this.TimeChartEntries = timeEntries;
        }

        public Dictionary<DateTime, (double averageMistakes, double averageSeconds)> GroupDetails()
        {
            Dictionary<DateTime, (double averageMistakes, double averageSeconds)> detailDictionary = new Dictionary<DateTime, (double, double)>();
            IEnumerable<(DateTime Date, List<TrainingStatisticDetails>)> groups;

            switch (this.SelectedGroupOption.GroupOption)
            {
                case GroupOption.Day:
                    groups = StatisticDetails.Where(d => d.Date.Year == this.SelectedDate.Year && d.Date.Month == this.SelectedDate.Month && d.Date.Day == this.SelectedDate.Day).GroupBy(d => d.Date.Hour).Select(g => (g.First().Date.Date.AddHours(g.First().Date.Hour), g.ToList()));
                    break;
                case GroupOption.Month:
                    groups = StatisticDetails.Where(d => d.Date.Year == this.SelectedDate.Year && d.Date.Month == this.SelectedDate.Month).GroupBy(d => d.Date.Day).Select(g => (g.First().Date.Date, g.ToList()));
                    break;
                case GroupOption.Year:
                    groups = StatisticDetails.Where(d => d.Date.Year == this.SelectedDate.Year).GroupBy(d => d.Date.Month).Select(g => (g.First().Date.Date.AddDays(-(g.First().Date.Day - 1)), g.ToList()));
                    break;
                case GroupOption.SessionsDay:
                    //HACK & TODO: optimization with <10 data entries
                    groups = StatisticDetails.Where(d => d.Date.Year == this.SelectedDate.Year && d.Date.Month == this.SelectedDate.Month && d.Date.Day == this.SelectedDate.Day).Select(d => new Grouping<DateTime, TrainingStatisticDetails>(d.Date, new[] { d })).Select((g, index) => (new DateTime().AddDays(index), g.ToList()));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(this.SelectedGroupOption));
            }

            foreach (var group in groups)
            {
                detailDictionary.Add(group.Date, (group.Item2.Average(d => d.TotalErrors), group.Item2.Average(d => d.SessionDurationSeconds)));
            }

            return detailDictionary;
        }

        public TrainingStatisticDetails[] StatisticDetails
        {
            get => statisticDetails;
            set
            {
                statisticDetails = value;
                UpdateCharts();
                this.OnPropertyChanged();
            }
        }

        public Dictionary<DateTime, (double averageMistakes, double averageSeconds)> Groups { get; private set; }
        public DateTime SelectedDate
        {
            get => selectedDate;
            private set
            {
                selectedDate = value;
                this.OnPropertyChanged(nameof(this.Year));
                this.OnPropertyChanged(nameof(this.Month));
                this.OnPropertyChanged(nameof(this.Day));

                this.AddYearCommand.ChangeCanExecute();
                this.SubtractYearCommand.ChangeCanExecute();
                this.AddMonthCommand.ChangeCanExecute();
                this.SubtractMonthCommand.ChangeCanExecute();
                this.AddDayCommand.ChangeCanExecute();
                this.SubtractDayCommand.ChangeCanExecute();

                this.UpdateCharts();
            }
        }

        public ExtendedCommand AddYearCommand { get; }

        public ExtendedCommand SubtractYearCommand { get; }

        public ExtendedCommand AddMonthCommand { get; }

        public ExtendedCommand SubtractMonthCommand { get; }

        public ExtendedCommand AddDayCommand { get; }

        public ExtendedCommand SubtractDayCommand { get; }

        public ObservableCollection<GroupOptionViewModel> GroupOptions { get => Enum.GetValues(typeof(GroupOption)).Cast<GroupOption>().Select(g => new GroupOptionViewModel(g)).ToObservableCollection(); }

        public GroupOptionViewModel SelectedGroupOption
        {
            get => groupOption;
            set
            {
                groupOption = value;
                this.UpdateCharts();
                this.OnPropertyChanged(nameof(this.YearSelectionVisible));
                this.OnPropertyChanged(nameof(this.MonthSelectionVisible));
                this.OnPropertyChanged(nameof(this.DaySelectionVisible));
            }
        }

        public ImageSource RightArrowImageSource { get => ImageSource.FromStream(() => new MemoryStream(imageRecourceService.GetImage("rightArrow.png"))); }
        public ImageSource LeftArrowImageSource { get => ImageSource.FromStream(() => new MemoryStream(imageRecourceService.GetImage("leftArrow.png"))); }
        public string Year { get => this.SelectedDate.ToString("yyyy"); }
        public string Month { get => this.SelectedDate.ToString("MMMM"); }
        public string Day { get => this.SelectedDate.ToString("dd"); }

        public bool DaySelectionVisible { get => this.SelectedGroupOption.GroupOption == GroupOption.Day || this.SelectedGroupOption.GroupOption == GroupOption.SessionsDay; }
        public bool MonthSelectionVisible { get => this.SelectedGroupOption.GroupOption == GroupOption.Day || this.SelectedGroupOption.GroupOption == GroupOption.Month || this.SelectedGroupOption.GroupOption == GroupOption.SessionsDay; }
        public bool YearSelectionVisible { get => this.SelectedGroupOption.GroupOption == GroupOption.Day || this.SelectedGroupOption.GroupOption == GroupOption.Month || this.SelectedGroupOption.GroupOption == GroupOption.Year || this.SelectedGroupOption.GroupOption == GroupOption.SessionsDay; }
    }
}
