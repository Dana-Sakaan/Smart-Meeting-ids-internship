using System;
using System.ComponentModel.DataAnnotations;


namespace Smart_Meeting.Models
{
    public enum RoomStatus
    {
        Available,
        Occupied
    }

    public class Room
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Please write room name…")]
        public string RoomName { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        [Required]
        public int Capacity { get; set; }
        [Required]
        public RoomStatus status { get; set; } = RoomStatus.Available;

        [Required]
        public required RoomFeatures RoomFeatures { get; set; }
    }
}
