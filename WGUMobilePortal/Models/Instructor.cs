
using SQLite;

namespace WGUMobilePortal.Models
{
    public class Instructor
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
    }

}