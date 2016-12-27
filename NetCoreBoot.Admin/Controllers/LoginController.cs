using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NetCoreBoot.Common;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreBoot.Admin.Controllers
{
    public class LoginController : Controller
    {


        private readonly WebHelper _userFactory;
        public LoginController(WebHelper helper)
        {
            this._userFactory = helper;
        }

        // GET: /<controller>/
        public IActionResult Index()
        { 

            return View();
        }

        
        public IActionResult GetAuthCode()
        {
            return View();
        }
    }
}
