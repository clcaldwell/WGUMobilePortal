using System;

using SQLite;

namespace WGUMobilePortal.Models
{
    public class Course // : ViewModels.BaseViewModel
    {
        [PrimaryKey, AutoIncrement] 
        public int Id { get; set; }

        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CourseStatus Status { get; set; }
        public bool Notify { get; set; }

        public int InstructorId { get; set; }
        public Nullable<int> NoteId { get; set; }
        public int AssessmentId { get; set; }

    }

    public enum CourseStatus
    {
        Started,
        Completed,
        Dropped,
        Planned
    }

}
