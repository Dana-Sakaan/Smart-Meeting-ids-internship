using Smart_Meeting.Models;
using System.ComponentModel.DataAnnotations;

namespace Smart_Meeting.Models
{
    public enum MeetingStatus
    {
        Upcoming,
        Inprogress,
        Completed,
        Canceled
    }
    public class Meeting
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public TimeOnly Time { get; set; }

        [Required]
        public int Duration { get; set; }


        public TimeOnly EndTime { get; set; }

        [Required]
        public MeetingStatus status { get; set; } = MeetingStatus.Upcoming;

        [Required]
        public int RoomID { get; set; }

        [Required]
        public string EmployeeID { get; set; }

        public Room? Room { get; set; }
        public Employee? Employee { get; set; }

        public MinutesOfMeeting? MinutesOfMeeting { get; set; }
        public List<Attendee> Attendees { get; set; } = new();
    }
}