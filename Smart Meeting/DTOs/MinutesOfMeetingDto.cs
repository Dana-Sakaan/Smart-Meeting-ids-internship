using AutoMapper;
using Smart_Meeting.Models;



namespace Smart_Meeting.DTOs
{
    public class MinutesOfMeetingDto
    {
        public int MeetingID { get; set; }
        public string Summary { get; set; }

        public string SummaryPdf { get; set; }

        public string AuthorId { get; set; }
        public string AuthorEmail { get; set; }
    }
}
