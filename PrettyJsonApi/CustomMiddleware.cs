using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;

namespace PrettyJsonApi
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<ApiKeySettings> _options;


        public CustomMiddleware(RequestDelegate next, IOptions<ApiKeySettings> options)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            var keyHeader = context.Request.Headers.ContainsKey("ApiKey")
                ? context.Request.Headers["ApiKey"].FirstOrDefault()
                : null;


            if (keyHeader == null || _options.Value.ApiKey != keyHeader)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Oops - badness!");
                return;
            }

            await _next.Invoke(context);
        }
    }
}
