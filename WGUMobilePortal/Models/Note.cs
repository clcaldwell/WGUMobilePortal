using MvvmHelpers;

using SQLite;

using System;

namespace WGUMobilePortal.Models
{
    public class Note : ObservableObject
    {
        public string Contents { get; set; }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}