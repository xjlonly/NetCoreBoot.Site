using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCoreBoot.IService;
using NetCoreBoot.Service;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Reflection;

namespace NetCoreBoot.Admin
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .AddJsonFile($"diservice.json", optional:true, reloadOnChange:true);
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            //配置文件读取依赖注入文件
            var requiredServices = new List<DIService>();
            Configuration.GetSection("DIServices").Bind(requiredServices);
            Assembly asmb_IService = Assembly.Load(new AssemblyName("NetCoreBoot.IService"));
            Assembly asmb_Service = Assembly.Load(new AssemblyName("NetCoreBoot.Service"));
            requiredServices.ForEach(rservice =>
            {
                services.Add(new ServiceDescriptor(serviceType: asmb_IService.GetType(rservice.ServiceType),
                                                   implementationType: asmb_Service.GetType(rservice.ImplementationType),
                                                   lifetime: rservice.Lifetime));
            });

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
                app.UseExceptionHandler("/Home/Error.html");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}.html/{id?}");
            });
        }
    }
}
