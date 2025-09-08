using Microsoft.AspNetCore.Http.Features;
using System;
using System.ComponentModel.DataAnnotations;


namespace Smart_Meeting.Models
{
    public enum RoomStatus
    {
        Available,
        UnderMaintenance
    }

    public class Room
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string RoomName { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        [Required]
        public int Capacity { get; set; }

        public RoomStatus status { get; set; } = RoomStatus.Available;

        public ICollection<RoomFeatures> RoomFeatures { get; set; }

    }
}
