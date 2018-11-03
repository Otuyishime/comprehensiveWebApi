using System;
using testWebAPI.Infrastructure.Attributes;
using testWebAPI.Models.Forms;

namespace testWebAPI.Models.Resources
{
    public class Room : Resource
    {
        [Sortable]
        [Searchable]
        public string Name { get; set; }

        [Sortable(Default = true)]  // make the rate the default sort property
        [SearchableDecimal]
        public Decimal Rate { get; set; }

        public Form Book { get; set; }
    }
}
