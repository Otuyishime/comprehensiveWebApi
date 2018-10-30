using System;
namespace testWebAPI.Models
{
    public class BookingRange
    {
        public DateTimeOffset StartAt { get; set; }

        public DateTimeOffset EndAt { get; set; }
    }
}
