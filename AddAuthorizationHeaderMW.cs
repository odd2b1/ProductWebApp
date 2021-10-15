using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductWebApp
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AddAuthorizationHeaderMW
    {
        private readonly RequestDelegate _next;

        public AddAuthorizationHeaderMW(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (string.IsNullOrEmpty(httpContext.Request.Headers["Authorization"]))
            {
                var token = httpContext.Request.Cookies["Auth"];
                httpContext.Request.Headers.Add("Authorization", "Bearer " + token);
            }
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AddAuthorizationHeaderMWExtensions
    {
        public static IApplicationBuilder UseAddAuthorizationHeaderMW(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AddAuthorizationHeaderMW>();
        }
    }
}
