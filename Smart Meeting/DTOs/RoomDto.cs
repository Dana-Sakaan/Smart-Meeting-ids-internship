using Smart_Meeting.Models;


namespace Smart_Meeting.DTOs
{
    public class RoomDto
    {
        public int ID { get; set; }
        public string RoomName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public string status { get; set; }

        public int Capacity { get; set; }

    }
}
