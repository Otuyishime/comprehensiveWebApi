using System;
namespace testWebAPI.Models.Resources
{
    public class RootResponse : Resource
    {
        public Link Info { get; set; }
        public Link Rooms { get; set; }
    }
}
