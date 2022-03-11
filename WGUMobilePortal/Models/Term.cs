using System;

using SQLite;

using WGUMobilePortal.Models;

namespace WGUMobilePortal.Models
{
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int CourseId { get; set; }
    }
}
