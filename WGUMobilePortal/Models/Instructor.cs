using System.ComponentModel.DataAnnotations;

using SQLite;

namespace WGUMobilePortal.Models
{
    public partial class Course
    {
        public class Instructor
        {
            [PrimaryKey, AutoIncrement]
            public int Id { get; set; }

            public string FirstName { get; set; }

            [Required(ErrorMessage = "Must have a {0}")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Must have a valid {0}")]
            [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid {0}")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "Must have a valid {0}")]
            [DataType(DataType.EmailAddress, ErrorMessage = "Invalid {0}")]
            public string EmailAddress { get; set; }
        }

    }

}
