using System;
using System.ComponentModel.DataAnnotations;

using SQLite;

namespace WGUMobilePortal.Models
{
    public class Note
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime TimeStamp { get; set; }

        [Required(ErrorMessage = "Note cannot be blank")]
        public string Contents { get; set; }
    }
}
