using System;
namespace testWebAPI.Models.Entities
{
    public class RoomEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Rate { get; set; }   // store the price of the room
    }
}
