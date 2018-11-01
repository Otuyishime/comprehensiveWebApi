using System;
using testWebAPI.Infrastructure.Attributes;

namespace testWebAPI.Models.Resources
{
    public class Room : Resource
    {
        [Sortable]
        [Searchable]
        public string Name { get; set; }

        [Sortable(Default = true)]  // make the rate the default sort property
        [Searchable]
        public Decimal Rate { get; set; }
    }
}
