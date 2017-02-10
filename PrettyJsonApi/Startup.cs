using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace PrettyJsonApi
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
           
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApiKeySettings>(Configuration.GetSection("ApiKeySettings"));
            services.AddScoped<ISomeService, SomeService>();
            services.AddSingleton<RouteKeyShow>();
            services.AddRouting();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, 
            IOptions<ApiKeySettings> signingSettings, RouteKeyShow routeKeyShow)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            
            app.UseMiddleware<CustomMiddleware>();

            app.UseRouter(routeKeyShow.AddKeyRoute);

            app.UseRouter(router =>
            {
                router.MapGet("hi", async (request, response, routeData) =>
                {
                    await response.WriteAsync("gday");
                });

                router.MapPost("pretty/{language}", async (request, response, routeData) =>
                {
                    using (var streamReader = new StreamReader(request.HttpContext.Request.Body))
                    {
                        var json = await streamReader.ReadToEndAsync();

                        var language = routeData.Values["language"];

                        var basePath = $"{request.Scheme}://{request.Host}";

                        var pattern = @"
                            <!DOCTYPE html>
                                <html>
                                <head>	
	                                <link href=""{0}/prism.css"" rel=""stylesheet"" />
                                </head>
                                <body>	
	                                <script src=""{0}/prism.js""></script>
                                    <pre><code class=""language-{2}"">{1}</code></pre>
                                </body>
                                </html>";
                        var result = string.Format(pattern, basePath, json, language);

                        await response.WriteAsync(result);
                    }
                });
            });
        }
    }
}
