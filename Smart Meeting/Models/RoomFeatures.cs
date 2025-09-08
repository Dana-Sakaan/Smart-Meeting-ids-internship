using AutoMapper.Features;
using System.ComponentModel.DataAnnotations;

namespace Smart_Meeting.Models
{
    public class RoomFeatures
    {
        [Key]
        public int ID { get; set; }
        public int RoomID { get; set; }
        public Room Room { get; set; }

        public int FeatureID { get; set; }
        public AvailableFeatures AvailableFeatures { get; set; }

    }
}
