﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using NetCoreBoot.Common;
using Microsoft.AspNetCore.Http;

namespace ManagerBoot.Admin
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .AddJsonFile("diservice.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            //services.AddSession(option =>
            //{
            //    option.IdleTimeout = TimeSpan.FromMinutes(30);
            //});

            //配置服务依赖注入
            var requiredServices = new List<DIService>();
            Configuration.GetSection("DIServices")?.Bind(requiredServices);
            if (requiredServices == null || requiredServices.Count == 0)
            {
                throw new Exception("注入服务配置有误，请检查服务配置！");
            }
            //加载程序集
            Assembly asmb_IService = Assembly.Load(new AssemblyName("NetCoreBoot.IService"));
            Assembly asmb_Service = Assembly.Load(new AssemblyName("NetCoreBoot.Service"));
            requiredServices.ForEach(rservice =>
            {
                services.Add(new ServiceDescriptor(serviceType: asmb_IService.GetType(rservice.ServiceType),
                                                   implementationType: asmb_Service.GetType(rservice.ImplementationType),
                                                   lifetime: rservice.Lifetime));
            });

            //注入HttpContextAccessor
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //注入公共操作服务
            services.AddTransient<WebHelper>();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider isp)
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

            app.UseStaticFiles();
            NetCoreBoot.Common.HttpContext.ServiceProvider = isp;
            app.UseStaticFiles();
            //app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}.html"
                    );

                routes.MapRoute(
                    name: "api",
                    template: "{controller}/{action}.do"
                    );

                routes.MapRoute(
                    name: "replenish",
                    template: "{controller=Login}/{action=Index}/{id?}"
                    );

            });
        }
    }
}
