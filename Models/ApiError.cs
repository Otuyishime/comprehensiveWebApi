using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace testWebAPI.Models
{
    public class ApiError
    {
        public string Message { get; set; }
        public string Detail { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string StackTrace { get; set; }
    }
}
