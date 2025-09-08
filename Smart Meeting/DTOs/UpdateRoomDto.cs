using Smart_Meeting.Models;
using System.ComponentModel.DataAnnotations;

namespace Smart_Meeting.DTOs
{
    public class UpdateRoomDto
    {
        public string RoomName { get; set; }

        public string Description { get; set; }

        public string status { get; set; }

        public int Capacity { get; set; }
    }
}
