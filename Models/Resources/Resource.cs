using System;
using Newtonsoft.Json;

namespace testWebAPI.Models.Resources
{
    public abstract class Resource
    {
        [JsonProperty(Order = -2)]
        public string Href { get; set; }    // This is for a self-referential link
    }
}
