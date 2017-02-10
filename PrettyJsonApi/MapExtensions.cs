using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace PrettyJsonApi
{
    public static class MapExtensions
    {
        public static void MapGetExtended<T>(this IRouteBuilder builder,
            string template)
            where T : IRouteWrapper
        {
            builder.MapGet(template, async (request, response, arg3) =>
            {
                var resolved = request.HttpContext.RequestServices.GetService<T>();
                await resolved.Run(request, response, arg3);
            });
        }

        public static void MapPostExtended<T>(this IRouteBuilder builder,
            string template)
            where T : IRouteWrapper
        {
            builder.MapPost(template, async (request, response, arg3) =>
            {
                var resolved = request.HttpContext.RequestServices.GetService<T>();
                await resolved.Run(request, response, arg3);
            });
        }

        public static void MapPutExtended<T>(this IRouteBuilder builder,
            string template)
            where T : IRouteWrapper
        {
            builder.MapPut(template, async (request, response, arg3) =>
            {
                var resolved = request.HttpContext.RequestServices.GetService<T>();
                await resolved.Run(request, response, arg3);
            });
        }

        public static void MapDeleteExtended<T>(this IRouteBuilder builder,
            string template)
            where T : IRouteWrapper
        {
            builder.MapDelete(template, async (request, response, arg3) =>
            {
                var resolved = request.HttpContext.RequestServices.GetService<T>();
                await resolved.Run(request, response, arg3);
            });
        }
    }

    public interface IRouteWrapper
    {
        Task Run(HttpRequest request, HttpResponse response, RouteData routeData);
    }
}
