using System.ComponentModel.DataAnnotations;

namespace Smart_Meeting.Models
{
    public class Notification
    {
        [Key]
        public int ID { get; set; }

        public string  Message { get; set; } = string.Empty;
        public string EmployeeID { get; set; }

        public Employee? Employee { get; set; }

    }
}
