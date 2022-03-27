using MvvmHelpers;

using SQLite;

using System;

namespace WGUMobilePortal.Models
{
    public enum AssessmentStyle
    {
        Objective,
        Performance
    }

    public class Assessment : ObservableObject
    {
        private DateTime _minimumEndDate;
        private DateTime _startDate;
        public int CourseId { get; set; }

        public DateTime EndDate { get; set; }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Ignore]
        public DateTime MinimumEndDate
        {
            get => _minimumEndDate;
            set
            {
                SetProperty(ref _minimumEndDate, value);
                OnPropertyChanged(nameof(MinimumEndDate));
            }
        }

        [Ignore]
        public DateTime MinimumStartDate { get => DateTime.Today.AddDays(-60).Date; }

        public string Name { get; set; }
        public bool ShouldNotify { get; set; }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                SetProperty(ref _startDate, value);
                OnPropertyChanged(nameof(StartDate));
                MinimumEndDate = StartDate.AddDays(1).Date;
            }
        }

        public AssessmentStyle Style { get; set; }
    }
}