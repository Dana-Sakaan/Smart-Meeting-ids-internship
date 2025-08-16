using System.ComponentModel.DataAnnotations;

namespace Smart_Meeting.Models
{
    public class Attendee
    {
        [Key]
        public int ID { get; set; }

        public string EmployeeID { get; set; } = string.Empty;

        public int MeetingID { get; set; }

        public Employee? Employee { get; set; }

        public Meeting? Meeting { get; set; }
    }
}
