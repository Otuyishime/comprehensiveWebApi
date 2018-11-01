using System;
using testWebAPI.Infrastructure.Attributes;
using testWebAPI.Models.Entities;

namespace testWebAPI.Models.Resources
{
    public class Opening
    {
        [Sortable(EntityProperty = nameof(OpeningEntity.RoomId))]
        public Link Room { get; set; }

        [Sortable(Default = true)]
        public DateTimeOffset StartAt { get; set; }

        [Sortable]
        public DateTimeOffset EndAt { get; set; }

        [Sortable]
        public decimal Rate { get; set; }
    }
}
