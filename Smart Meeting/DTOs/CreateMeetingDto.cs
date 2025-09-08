using System.ComponentModel.DataAnnotations;

namespace Smart_Meeting.DTOs
{
    public class CreateMeetingDto
    {

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateOnly Date { get; set; }

        public TimeOnly Time { get; set; }

        public int Duration { get; set; }

        public int RoomID { get; set; }

        public string EmployeeID { get; set; }
    }
}
