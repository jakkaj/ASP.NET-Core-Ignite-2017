using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PrettyJsonApi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouter(router =>
            {
                router.MapPost("pretty", async (request, response, RouteData) =>
                {
                    using (var streamReader = new StreamReader(request.HttpContext.Request.Body))
                    {
                        var json = await streamReader.ReadToEndAsync();

                        var basePath = $"{request.Scheme}://{request.Host}";

                        var pattern = @"
                            <!DOCTYPE html>
                                <html>
                                <head>	
	                                <link href=""{0}/prism.css"" rel=""stylesheet"" />
                                </head>
                                <body>	
	                                <script src=""{0}/prism.js""></script>
                                    <pre><code class=""language-csharp"">{1}</code></pre>
                                </body>
                                </html>";
                        var result = string.Format(pattern, basePath, json);

                        await response.WriteAsync(result);
                    }
                });
            });
        }
    }
}
