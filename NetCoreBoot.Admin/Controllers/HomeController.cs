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
        private IAccountService _memberService;
        public IAccountService MemberService
        {
            get { return _memberService; }
            set
            {
                this._memberService = value;
            }
        }

        IUserServices bc { get; set; }
 
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Index1()
        {
            IServiceProvider serviceProvider = new ServiceCollection().BuildServiceProvider();
            string str = "";

            str = $"serviceProvider.GetService<IUserServices>(): {_memberService}";
            str += $"<br/>serviceProvider.GetService<IAccountService>(): {bc}";
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
