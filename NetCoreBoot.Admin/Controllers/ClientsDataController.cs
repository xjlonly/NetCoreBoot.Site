using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreBoot.CommonApplication;


namespace NetCoreBoot.Admin.Controllers
{
    public class ClientsDataController : WebController
    {
        // GET: /<controller>/
        [HttpGet]
        public IActionResult GetClientsDataJson()
        {
            var data = 
            return View();
        }


        private object GetMenuList()
        {
            var roleId = AdminUtils.GetCurrentCookie().RoleId;

        }
    }
}
