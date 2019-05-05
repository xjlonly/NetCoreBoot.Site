using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreBoot.Entity.CommonModel;

namespace BootManager.Admin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //注入数据库连接串模型
            services.Configure<DbOption>("DbBoot", Configuration.GetSection("DbOption"));
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //CSRF 
            services.AddAntiforgery(option =>
            {
                option.FormFieldName = "AntiforgeryKey_xjlonly";
                option.HeaderName = "X-CSRF-TOKEN-xjlonly";
                option.SuppressXFrameOptionsHeader = false;
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //Autofac注入服务
            var containerBuilder = new ContainerBuilder();
            //模块化注入模块化的好处就是把所有相关的注册配置都放在一个类中，使代码更易于维护和配置
            containerBuilder.RegisterModule<DefaultModuleRegister>();
            containerBuilder.Populate(services);
            var container = containerBuilder.Build();
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(ConfigureRoutes);
        }


        public void ConfigureRoutes(IRouteBuilder builder)
        {
            builder.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}"
            );
            builder.MapRoute(
                name:"statichtml",
                template:"{controller=Home}/{action=Index}.html/{id?}"
                );

            builder.MapRoute(
                name: "apido",
                template: "{controller=Home}/{action=Index}.html/{id?}"
            );
        }
    }
}
