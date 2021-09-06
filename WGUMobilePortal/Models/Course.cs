using System;
using System.ComponentModel.DataAnnotations;

using SQLite;

using SQLiteNetExtensions.Attributes;

namespace WGUMobilePortal.Models
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Must have a {0}")]
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Status Status { get; set; }

        public bool Notify { get; set; }

        [ManyToOne]
        public Instructor CourseInstructor { get; set; }

        [OneToMany]
        public Note CourseNote { get; set; }

        [OneToMany]
        public Assessment Assessment { get; set; }

    }

    public enum Status
    {
        Started,
        Completed,
        Dropped,
        Planned
    }

}
