using System;
using System.ComponentModel.DataAnnotations;

using SQLite;

using SQLiteNetExtensions.Attributes;

namespace WGUMobilePortal.Models
{
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Required(ErrorMessage = "Term must have a {0}")]
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [OneToMany]
        public Course Course { get; set; }
    }
}
