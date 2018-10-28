using System;
using Newtonsoft.Json;

namespace testWebAPI.Models.Resources
{
    public abstract class Resource : Link
    {
        [JsonIgnore]
        public Link Self { get; set; }    // This is for a self-referential link
    }
}
