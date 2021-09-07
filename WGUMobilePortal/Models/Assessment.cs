using System;
using System.ComponentModel.DataAnnotations;

using SQLite;

using SQLiteNetExtensions.Attributes;

namespace WGUMobilePortal.Models
{
    public class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Required(ErrorMessage = "Assessment must have a {0]")]
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Must select the style of assessment")]
        public AssessmentStyle Style { get; set; } 

        [ManyToOne]
        public Course Course { get; set; }

    }

    public enum AssessmentStyle
    {
        Objective,
        Performance
    }
}
