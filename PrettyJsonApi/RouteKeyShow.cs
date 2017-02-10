using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace PrettyJsonApi
{
    public class RouteKeyShow
    {
        private readonly IServiceProvider _serviceProvider;

        public RouteKeyShow(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public void AddKeyRoute(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapGet("key", async (request, response, arg3) =>
            {
                var someService = request.HttpContext.RequestServices.GetService<ISomeService>();
                
                await response.WriteAsync($"The key used was: {someService.HeaderKey}");
            });
        }
    }
}
