using Microsoft.AspNetCore.Http.Features;
using System.ComponentModel.DataAnnotations;

namespace Smart_Meeting.Models
{
    public class AvailableFeatures
    {
        public int ID { get; set; }

        [Required]
        public string Feature { get; set; } = string.Empty;

        public ICollection<RoomFeatures> RoomFeatures { get; set; }

    }
}
