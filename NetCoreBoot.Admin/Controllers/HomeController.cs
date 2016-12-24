using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NetCoreBoot.IService;
using NetCoreBoot.Service;
using Microsoft.Extensions.Configuration;


namespace NetCoreBoot.Admin.Controllers
{
    public class HomeController : Controller
    {
        private IAccountService accountService { get; set; }

        private IUserService userServices { get; set; }

        public HomeController(IAccountService accountService, IUserService userServices)
        {
            this.accountService = accountService;
            this.userServices = userServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Index1()
        {
            var builder = new ConfigurationBuilder();
            var Config = builder.AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)?.Build();
            var cong = Config["DBType"];

            string str = String.Empty;
            string userid = "";
            bool result = accountService.CheckLogin("admin", "123456", out userid);
            str = $"serviceProvider.GetService<IUserServices>():{result}";
            str += $"<br/>serviceProvider.GetService<IAccountService>(): {userid}";
            ViewBag.STR = str;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
