using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace ManagerBoot.Admin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var host = new WebHostBuilder()
            //    .UseKestrel()
            //    .UseContentRoot(Directory.GetCurrentDirectory())
            //    .UseIISIntegration()
            //    .UseUrls("http://*:5000")
            //    .UseStartup<Startup>()
            //    .UseApplicationInsights()
            //    .Build();

            //host.Run();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            //加载配置文件入口之一
                .ConfigureAppConfiguration((hostContext, config) => {
                    var env = hostContext.HostingEnvironment;
                    config.AddJsonFile("appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile(path: "diservice.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();
                })
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders(); //清除日志提供程序 引入第三方日志组件时使用
                    logging.SetMinimumLevel(LogLevel.Information); //设置日志级别为info
                    logging.AddConsole(); //添加日志控制程序——控制台上
                }).UseNLog();
    }
}
