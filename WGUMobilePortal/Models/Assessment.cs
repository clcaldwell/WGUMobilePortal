using System;

using SQLite;

namespace WGUMobilePortal.Models
{
    public class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public AssessmentStyle Style { get; set; } 
        public Course Course { get; set; }
    }

    public enum AssessmentStyle
    {
        Objective,
        Performance
    }
}
