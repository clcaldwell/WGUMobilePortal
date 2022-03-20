using SQLite;

using System;

namespace WGUMobilePortal.Models
{
    public enum AssessmentStyle
    {
        Objective,
        Performance
    }

    public class Assessment
    {
        public int CourseId { get; set; }

        public DateTime EndDate { get; set; }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public AssessmentStyle Style { get; set; }
    }
}