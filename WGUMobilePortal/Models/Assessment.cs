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
        public int CourseId { get; set; }

        public DateTime DueDate { get; set; }

        public bool DueDateShouldNotify { get; set; }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
        public AssessmentStyle Style { get; set; }
    }
}