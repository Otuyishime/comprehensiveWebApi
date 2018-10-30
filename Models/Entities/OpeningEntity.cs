using System;
namespace testWebAPI.Models.Entities
{
    public class OpeningEntity : BookingRange
    {
        public Guid RoomId { get; set; }

        public int Rate { get; set; }
    }
}
