using AutoMapper;
using Smart_Meeting.Models;



namespace Smart_Meeting.DTOs
{
    public class MinutesOfMeetingDto
    {
        public string Summary { get; set; } = string.Empty;

        public string SummaryPdf { get; set; } = string.Empty;

        public string AuthorId { get; set; }
    }
}
