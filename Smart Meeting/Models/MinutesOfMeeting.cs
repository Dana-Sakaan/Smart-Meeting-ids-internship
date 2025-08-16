using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Meeting.Models
{
    public class MinutesOfMeeting
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Summary { get; set; } = string.Empty;

        public string SummaryPdf { get; set; } = string.Empty;

        public int MeetingID { get; set; }
        public Meeting? Meeting { get; set; }

        public string AuthorId { get; set; } = string.Empty;
        public Employee? Author { get; set; }

    }
}
