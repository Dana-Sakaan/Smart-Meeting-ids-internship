using Smart_Meeting.Models;


namespace Smart_Meeting.DTOs
{
    public class MeetingDto
    {
        public int ID { get; set; }
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateOnly Date { get; set; }

        public TimeOnly Time { get; set; }
        public string EmployeeID { get; set; }

        public int Duration { get; set; }

        public TimeOnly EndTime { get; set; }

        public string RoomName { get; set; }

        public string status { get; set; }

        public string CreaterFirstName { get; set; }
        public string CreaterLastName { get; set; }

       public string AuthorId { get; set; }
    }
}
