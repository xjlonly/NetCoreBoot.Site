using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NetCoreBoot.IService;
using NetCoreBoot.Service;


namespace NetCoreBoot.Admin.Controllers
{
    public class HomeController : Controller
    {
        private IAccountService accountService { get; set; }

        private IUserServices userServices { get; set; }

        public HomeController(IAccountService accountService, IUserServices userServices)
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
            string str = String.Empty;
            str = $"serviceProvider.GetService<IUserServices>(): {accountService}";
            str += $"<br/>serviceProvider.GetService<IAccountService>(): {userServices}";
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
