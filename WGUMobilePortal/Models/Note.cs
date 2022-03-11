using System;

using SQLite;

namespace WGUMobilePortal.Models
{
    public class Note
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime TimeStamp { get; set; }
        public string Contents { get; set; }
    }
}
