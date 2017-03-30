using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NetCoreBoot.IService;
using NetCoreBoot.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using NetCoreBoot.Common;
using NetCoreBoot.CommonApplication;
using System.ComponentModel.Design;

namespace NetCoreBoot.Admin.Controllers
{
    public class HomeController : WebController 
    {
        private IAccountService accountService { get; set; }

        private IUserService userServices { get; set; }

        private WebHelper webHelper { get; set; }
        public HomeController(IAccountService accountService, IUserService userServices, WebHelper webHelper)
        {
            this.accountService = accountService;
            this.userServices = userServices;
            this.webHelper = webHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Index1()
        {
            string str = String.Empty;
            string userid = "";
            
            webHelper.SetCookie("SET", userid);
            //str = $"serviceProvider.GetService<IUserServices>():{result}";
            str += $"/r<br/>serviceProvider.GetService<IAccountService>(): {userid}";
            str += $"<><><><>>>>>>>>>>>>>>>>{webHelper.GetCookie("SET")}";
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

        [HttpGet]
        public ActionResult Default()
        {
            return View();
        }
        
    }
}
