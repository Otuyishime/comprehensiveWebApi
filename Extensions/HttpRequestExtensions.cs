using System;
using Microsoft.AspNetCore.Http;
using testWebAPI.Infrastructure.Etags;

namespace testWebAPI.Extensions
{
    public static class HttpRequestExtensions
    {
        public static IEtagHandlerFeature GetEtagHandler(this HttpRequest request)
            => request.HttpContext.Features.Get<IEtagHandlerFeature>();
    }
}
