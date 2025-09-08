namespace Smart_Meeting.DTOs
{
    public class CreateRoomDto
    {
        public string RoomName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public int Capacity { get; set; }

    }
}
