using System;
using Microsoft.AspNetCore.Mvc.Filters;
using testWebAPI.Filters;

namespace testWebAPI.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class EtagAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable => true;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
            => new EtagHeaderFilter();
    }
}
