using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using testWebAPI.Models;

namespace testWebAPI.Filters
{
    public class JsonExceptionFilter: IExceptionFilter
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public JsonExceptionFilter(IHostingEnvironment hostingEnvironment){
            _hostingEnvironment = hostingEnvironment;
        }

        public void OnException(ExceptionContext context)
        {
            var error = new ApiError();

            // Detect the hosting environment to return a proper response
            if (_hostingEnvironment.IsDevelopment())
            {
                error.Message = context.Exception.Message;
                error.Detail = context.Exception.StackTrace;
            }else{
                error.Message = "A Server Error Occured";
                error.Detail = context.Exception.Message;
            }

            context.Result = new ObjectResult(error)
            {
                StatusCode = 500
            };
        }
    }
}
