using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace PrettyJsonApi
{
    public class RouteKeyShowWrapper : IRouteWrapper
    {
        private readonly ISomeService _someService;

        public RouteKeyShowWrapper(ISomeService someService)
        {
            _someService = someService;
        }
        public async Task Run(HttpRequest request, HttpResponse response, RouteData routeData)
        {
            await response.WriteAsync($"The key used was: {_someService.HeaderKey}");
        }
    }
}
