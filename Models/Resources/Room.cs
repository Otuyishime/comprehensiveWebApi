using System;
namespace testWebAPI.Models.Resources
{
    public class Room : Resource
    {
        public string Name { get; set; }
        public Decimal Rate { get; set; }
    }
}
