using System;
using testWebAPI.Infrastructure.Attributes;

namespace testWebAPI.Models.Resources
{
    public class Room : Resource
    {
        [Sortable]
        public string Name { get; set; }

        [Sortable(Default = true)]  // make the rate the default sort property 
        public Decimal Rate { get; set; }
    }
}
