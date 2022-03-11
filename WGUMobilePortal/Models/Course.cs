using System;

namespace WGUMobilePortal.Models
{
    public class Course // : ViewModels.BaseViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CourseStatus Status { get; set; }
        public bool Notify { get; set; }

        public Instructor Instructor { get; set; }
        public Note Note { get; set; }
        public Assessment Assessment { get; set; }

    }

    public enum CourseStatus
    {
        Started,
        Completed,
        Dropped,
        Planned
    }

}
