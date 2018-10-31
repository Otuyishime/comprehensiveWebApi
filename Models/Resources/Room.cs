using System;
using testWebAPI.Infrastructure.Attributes;

namespace testWebAPI.Models.Resources
{
    public class Room : Resource
    {
        [Sortable]
        public string Name { get; set; }

        [Sortable]
        public Decimal Rate { get; set; }
    }
}
