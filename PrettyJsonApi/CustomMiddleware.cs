using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PrettyJsonApi
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;


        public CustomMiddleware(RequestDelegate next)
        {
            _next = next;

        }

        public async Task Invoke(HttpContext context)
        {
            var auth = context.User;

            if (auth == null || !auth.Identity.IsAuthenticated)
            {
                context.Response.StatusCode = 401;
                return;
            }

            await _next.Invoke(context);
        }
    }
}
