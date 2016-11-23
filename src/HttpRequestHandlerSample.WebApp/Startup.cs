using System;
using System.Collections.Generic;
using HttpRequestHandlerSample.WebApp.Middlewares;
using HttpRequestHandlerSample.WebApp.Settings;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HttpRequestHandlerSample.WebApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseDefaultFiles(new DefaultFilesOptions());
            app.UseStaticFiles();

            var settings = Configuration.Get<HttpRequestHeaderHandlerSettings>("httpRequestHeaderHandler");
            app.UseHttpRequestHeaderHandler(settings.Options);

            //app.UseHttpRequestHeaderHandler(o => o.Headers.Add(new HttpRequestHeader()
            //                                                  {
            //                                                      Url = "http://localhost:26895/",
            //                                                      Prefixes = { "api" }
            //                                                  }));
        }
    }
}