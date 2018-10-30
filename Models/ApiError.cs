﻿using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace testWebAPI.Models
{
    public class ApiError
    {
        public ApiError()
        {
        }

        public ApiError(ModelStateDictionary modelState)
        {
            Message = "Invalid parameters.";
            Detail = modelState.FirstOrDefault(m => m.Value.Errors.Any())
                               .Value.Errors.FirstOrDefault().ErrorMessage;
        }

        public string Message { get; set; }
        public string Detail { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string StackTrace { get; set; }
    }
}
