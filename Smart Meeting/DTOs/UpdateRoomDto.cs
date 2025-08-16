using Smart_Meeting.Models;
using System.ComponentModel.DataAnnotations;

namespace Smart_Meeting.DTOs
{
    public class UpdateRoomDto
    {
        public RoomStatus status { get; set; }
    }
}
