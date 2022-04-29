using MvvmHelpers;

using SQLite;

using System;

namespace WGUMobilePortal.Models
{
    public enum CourseStatus
    {
        Started,
        Completed,
        Dropped,
        Planned
    }

    public class Course : ObservableObject
    {
        public DateTime EndDate { get; set; }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string InstructorName { get; set; }
        public string InstructorPhone { get; set; }
        public string InstructorEmail { get; set; }
        public string Name { get; set; }
        public int NoteId { get; set; }
        public int ObjectiveAssessmentId { get; set; }
        public int PerformanceAssessmentId { get; set; }
        public DateTime StartDate { get; set; }
        public CourseStatus Status { get; set; }
        public int TermId { get; set; }

        public bool EndDateShouldNotify { get; set; }
        public bool StartDateShouldNotify { get; set; }
    }
}