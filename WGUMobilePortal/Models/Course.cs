using System;

using SQLite;

namespace WGUMobilePortal.Models
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CourseStatus Status { get; set; }
        public bool Notify { get; set; }

        public int InstructorId { get; set; }
        public int? NoteId { get; set; }
        public int AssessmentId { get; set; }
        public int? TermId { get; set; }
    }

    public enum CourseStatus
    {
        Started,
        Completed,
        Dropped,
        Planned
    }

}
