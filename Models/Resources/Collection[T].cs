using System;
namespace testWebAPI.Models.Resources
{
    public class Collection<T> : Resource
    {
        public T[] Value { get; set; }
    }
}
