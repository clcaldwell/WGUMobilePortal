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

    public class Course
    {
        public DateTime EndDate { get; set; }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int InstructorId { get; set; }
        public string Name { get; set; }
        public int NoteId { get; set; }
        public bool Notify { get; set; }
        public int ObjectiveAssessmentId { get; set; }
        public int PerformanceAssessmentId { get; set; }
        public DateTime StartDate { get; set; }
        public CourseStatus Status { get; set; }
        public int TermId { get; set; }
    }
}